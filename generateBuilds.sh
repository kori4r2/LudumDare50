#! /bin/sh

PROJECT_NAME=""
VERSION=""
UNITY_EXECUTABLE=""

if [ "$#" -ne 1 ] && [ "$#" -ne 2 ]; then
	echo "Usage: $0 PATH_TO_UNITY_EXECUTABLE VERSION_OVERRIDE(optional)" >&2
	exit 1
elif [ ! -e ProjectSettings/ProjectSettings.asset ]; then
	echo "Current folder is not a Unity Project!"
	exit 2
fi

UNITY_EXECUTABLE="$1"
echo "Using unity executable: $1"
if [ "$#" -ne 2 ]; then
	VERSION=$(grep bundleVersion ProjectSettings/ProjectSettings.asset | sed 's/.*bundleVersion: //')
else
	VERSION="$2"
fi

PROJECT_NAME=$(grep productName ProjectSettings/ProjectSettings.asset | sed 's/.*productName: //' | sed 's/ //g')

echo "Building project $PROJECT_NAME for version $VERSION"

mkdir -p $PWD/Build
#mkdir -p $PWD/Build/Linux
#mkdir -p $PWD/Build/Windows
mkdir -p $PWD/Build/Web
mkdir -p $PWD/Build/Android

# Build Linux
#rm -rf $PWD/Build/Linux/*
#$UNITY_EXECUTABLE -quit -projectPath $PWD -batchmode -nographics -buildTarget Linux64 -buildLinux64Player $PWD/Build/Linux/${PROJECT_NAME}.x86_64
#echo -e "\n\n\n\tLinux build finished!\n\n\n"
# Build Windows
#rm -rf $PWD/Build/Windows/*
#$UNITY_EXECUTABLE -quit -projectPath $PWD -batchmode -nographics -buildTarget Win64 -buildWindows64Player $PWD/Build/Windows/${PROJECT_NAME}.exe
#echo -e "\n\n\n\tWindows build finished!\n\n\n"
# Build Web
rm -rf $PWD/Build/Web/*
$UNITY_EXECUTABLE -quit -projectPath $PWD -batchmode -nographics -buildTarget WebGL -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildWebGL $PWD/Build/Web/
echo -e "\n\n\n\tWebGL build finished!\n\n\n"
# Build Android
rm -rf $PWD/Build/Android/*
$UNITY_EXECUTABLE -quit -projectPath $PWD -batchmode -nographics -buildTarget Android -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildAndroid $PWD/Build/Android/${PROJECT_NAME}.apk
echo -e "\n\n\n\tAndroid build finished!\n\n\n"

#cd Build/Linux
#rm -f ../${PROJECT_NAME}${VERSION}_Linux.zip
#zip -r ../${PROJECT_NAME}${VERSION}_Linux.zip ./
#cd ../Windows
#rm -f ../${PROJECT_NAME}${VERSION}_Windows.zip
#zip -r ../${PROJECT_NAME}${VERSION}_Windows.zip ./
cd ../Web
rm -f ../${PROJECT_NAME}${VERSION}_Web.zip
zip -r ../${PROJECT_NAME}${VERSION}_Web.zip ./
cd ../Android
rm -f ../${PROJECT_NAME}${VERSION}_Android.zip
zip -r ../${PROJECT_NAME}${VERSION}_Android.zip ./

cd ../..
