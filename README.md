# Fable bindings for Firebase

Fable bindings to make it easier to work with [Firebase](https://firebase.google.com/)

## To test samples

Samples will always use the code in the repository and not the actual published NuGet packages, just to make get an easier workflow. To build the samples you run

    ./fake.sh build target BuildSamples

That will create a `functions` folder in the `samples\Hello.Functions\` sample. This folder will contain a compiled node app that you can host on Firebase. To really try it you most likely need to set up your own Firebase project and and change the [.firebaserc](samples/Hello.functions/.firebaserc) file to your own project. You can test the functions locally by running the following command from the `samples\Hello.Functions` folder:

    npm run serve

## Future

There are definitely more to add, and I will try to add more support as I go along. First up will be cloud firestore, and the relevant dependencies for that. 