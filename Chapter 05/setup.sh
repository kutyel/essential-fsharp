dotnet new sln -o MyProject
cd MyProject
mkdir src
dotnet new classlib -lang F# -o src/MyProject
dotnet sln add src/MyProject/MyProject.fsproj
mkdir tests
dotnet new xunit -lang F# -o tests/MyProjectTests
dotnet sln add tests/MyProjectTests/MyProjectTests.fsproj
cd tests/MyProjectTests
dotnet add reference ../../src/MyProject/MyProject.fsproj
dotnet add package FsUnit
dotnet add package FsUnit.XUnit
dotnet build
dotnet test
