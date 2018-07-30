#load ".fake/build.fsx/intellisense.fsx"
open Fake.IO
open System.IO
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.Api
open Fake.JavaScript

let outputDir = ".output"
let summary = "Fable bindings for Firebase"
let project = "Fable.Import.Firebase"
let release =  ReleaseNotes.load "RELEASE_NOTES.md"
let gitOwner = "mastoj"
let gitRepo = "Fable.Import.Firebase"
let configuration = "Release"

let toLines (strs: string list) = String.concat "\n" strs

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ outputDir
    |> Shell.cleanDirs 
)

Target.create "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName =
        [ AssemblyInfo.Title (projectName)
          AssemblyInfo.Product project
          AssemblyInfo.Description summary
          AssemblyInfo.Version release.AssemblyVersion
          AssemblyInfo.FileVersion release.AssemblyVersion
          AssemblyInfo.Configuration configuration ]

    let getProjectDetails projectPath =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        ( System.IO.Path.GetDirectoryName(projectPath),
          (getAssemblyInfoAttributes projectName)
        )
    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (folderName, attributes) ->
        AssemblyInfoFile.createFSharp (Path.combine folderName "AssemblyInfo.fs") attributes
        )
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "NuGet" (fun _ ->
    Paket.pack(fun p ->
        { p with
            OutputPath = outputDir
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes})
)

Target.create "PublishNuGet" (fun _ ->
    Paket.push(fun p ->
        { p with
            PublishUrl = "https://www.nuget.org"
            WorkingDir = outputDir })
)

Target.create "GitHubRelease" (fun _ ->
    let token =
        match Environment.environVarOrDefault "github_token" "" with
        | s when not (System.String.IsNullOrWhiteSpace s) -> s
        | _ -> failwith "please set the github_token environment variable to a github personal access token with repro access."

    let files = !! ".output/**/*.*"

    token
    |> GitHub.createClientWithToken
    |> GitHub.draftNewRelease gitOwner gitRepo release.NugetVersion (release.SemVer.PreRelease <> None) release.Notes
    |> GitHub.uploadFiles files
    |> GitHub.publishDraft
    |> Async.RunSynchronously)

Target.create "CleanFunctionsFolders" (fun _ ->
    !! "samples/**/functions"
    |> Shell.cleanDirs 
)

Target.create "NpmInstall" (fun _ ->
    Npm.install id
)

Target.create "CopyNodeFiles" (fun _ ->
    let functionsFolder = "./samples/Hello.Functions/functions"
    let packageJsonSource = "./samples/Hello.Functions/package.json"
    
    Shell.copyFile functionsFolder packageJsonSource

    let target = DirectoryInfo.ofPath (functionsFolder + "/node_modules")
    let source = DirectoryInfo.ofPath "./samples/Hello.Functions/node_modules"
    DirectoryInfo.copyRecursiveTo true target source |> ignore
)

Target.create "CompileSamples" (fun _ ->
    let workingDirectory = Path.combine __SOURCE_DIRECTORY__ "samples/Hello.Functions"
    let result =
        DotNet.exec
            (DotNet.Options.withWorkingDirectory workingDirectory)
            "fable"
            "webpack -- -p"

    if not result.OK then failwithf "dotnet fable failed with code %i" result.ExitCode
)

Target.create "All" ignore
Target.create "BuildPackage" ignore
Target.create "BuildSamples" ignore
Target.create "Release" ignore

"Clean"
    ==> "AssemblyInfo"
    ==> "Build"
    ==> "Nuget"
    ==> "BuildPackage"
    ==> "All"

"Build"
    ==> "CleanFunctionsFolders"
    ==> "NpmInstall"
    ==> "CompileSamples"
    ==> "CopyNodeFiles"
    ==> "BuildSamples"

"BuildSamples"
    ==> "PublishNuget"
    ==> "GitHubRelease"
    ==> "Release"

Target.runOrDefault "Build"
