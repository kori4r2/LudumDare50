PROJECT_SETTINGS = $(CURDIR)/ProjectSettings/ProjectSettings.asset
override IS_UNITY_PROJECT = $(shell if [ ! -e $(PROJECT_SETTINGS) ]; then echo 0; else echo 1; fi)
ifeq ($(IS_UNITY_PROJECT),0)
$(error Current folder does not contain a Unity project!)
endif
PROJECT_NAME = $(shell grep productName $(PROJECT_SETTINGS) | sed 's/.*productName: //' | sed 's/ //g')
VERSION = $(shell grep bundleVersion $(PROJECT_SETTINGS) | sed 's/.*bundleVersion: //')
UNITY_EXECUTABLE =
UNITY_LICENSE=
BUILD_DIR = $(CURDIR)/Build
LINUX_BUILD_DIR = $(BUILD_DIR)/Linux
WINDOWS_BUILD_DIR = $(BUILD_DIR)/Windows
WEB_BUILD_DIR = $(BUILD_DIR)/Web
ANDROID_BUILD_DIR = $(BUILD_DIR)/Android

# Check that given variables are set and all have non-empty values,
# die with an error otherwise.
#
# Params:
#   1. Variable name(s) to test.
#   2. (optional) Error message to print.
# original source: https://stackoverflow.com/a/10858332
check_defined = \
    $(strip $(foreach 1,$1, \
        $(call __check_defined,$1,$(strip $(value 2)))))
__check_defined = \
    $(if $(value $1),, \
      $(error Undefined $1$(if $2, ($2))))

all : --check_variables --webgl --android

--linux: --build_linux --zip_linux_build
linux : --check_variables --linux
--linux-license : --build_linux-license --zip_linux_build
linux-license : --check_license --check_variables --linux-license

--windows : --build_windows --zip_windows_build
windows : --check_variables --windows
--windows-license : --build_windows-license --zip_windows_build
windows-license : --check_license --check_variables --windows-license

--webgl : --build_web --zip_web_build
webgl : --check_variables --webgl
--webgl-license : --build_webgl-license --zip_webgl_build
webgl-license : --check_license --check_variables --webgl-license

--android : --build_android --zip_android_build
android : --check_variables --android
--android-license : --build_android-license --zip_android_build
android-license : --check_license --check_variables --android-license

--check_license:
	$(call check_defined, UNITY_LICENSE, unity editor license for github actions workflows)

--check_variables :
	$(call check_defined, UNITY_EXECUTABLE, unity editor command or path to executable)
	@echo -e "\n\n\t/--------------------~~~~"
	@echo -e "\t| Using unity executable: $(UNITY_EXECUTABLE)"
	@echo -e "\t| Building project $(PROJECT_NAME) for version $(VERSION)"
	@echo -e "\t\\--------------------~~~~\n\n"

$(BUILD_DIR):
	@mkdir -p $(BUILD_DIR)

$(LINUX_BUILD_DIR): $(BUILD_DIR)
	@mkdir -p $(LINUX_BUILD_DIR)

$(WINDOWS_BUILD_DIR): $(BUILD_DIR)
	@mkdir -p $(WINDOWS_BUILD_DIR)

$(WEB_BUILD_DIR): $(BUILD_DIR)
	@mkdir -p $(WEB_BUILD_DIR)

$(ANDROID_BUILD_DIR): $(BUILD_DIR)
	@mkdir -p $(ANDROID_BUILD_DIR)

--build_linux: $(LINUX_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t Linux build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(LINUX_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Linux64 -buildLinux64Player $(LINUX_BUILD_DIR)/$(PROJECT_NAME).x86_64
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Linux build finished!"
	@echo -e "\t\\---------------------/\n\n"

--build_linux-license: $(LINUX_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t Linux build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(LINUX_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -manualLicenseFile $(UNITY_LICENSE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Linux64 -buildLinux64Player $(LINUX_BUILD_DIR)/$(PROJECT_NAME).x86_64
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Linux build finished!"
	@echo -e "\t\\---------------------/\n\n"

--build_windows: $(WINDOWS_BUILD_DIR)
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Windows build started"
	@echo -e "\t\\---------------------/\n\n"
	@rm -rf $(WINDOWS_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Win64 -buildWindows64Player $(WINDOWS_BUILD_DIR)/$(PROJECT_NAME).exe
	@echo -e "\n\n\t/-----------------------\\"
	@echo -e "\t Windows build finished!"
	@echo -e "\t\\-----------------------/\n\n"

--build_windows-license: $(WINDOWS_BUILD_DIR)
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Windows build started"
	@echo -e "\t\\---------------------/\n\n"
	@rm -rf $(WINDOWS_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -manualLicenseFile $(UNITY_LICENSE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Win64 -buildWindows64Player $(WINDOWS_BUILD_DIR)/$(PROJECT_NAME).exe
	@echo -e "\n\n\t/-----------------------\\"
	@echo -e "\t Windows build finished!"
	@echo -e "\t\\-----------------------/\n\n"

--build_web: $(WEB_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t WebGL build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(WEB_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget WebGL -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildWebGL $(WEB_BUILD_DIR)/
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t WebGL build finished!"
	@echo -e "\t\\---------------------/\n\n"

--build_web-license: $(WEB_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t WebGL build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(WEB_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -manualLicenseFile $(UNITY_LICENSE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget WebGL -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildWebGL $(WEB_BUILD_DIR)/
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t WebGL build finished!"
	@echo -e "\t\\---------------------/\n\n"

--build_android: $(ANDROID_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t Android build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(ANDROID_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Android -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildAndroid $(ANDROID_BUILD_DIR)/$(PROJECT_NAME).apk
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Android build finished!"
	@echo -e "\t\\---------------------/\n\n"

--build_android-license: $(ANDROID_BUILD_DIR)
	@echo -e "\n\n\t/-------------------\\"
	@echo -e "\t Android build started"
	@echo -e "\t\\-------------------/\n\n"
	@rm -rf $(ANDROID_BUILD_DIR)/*
	@$(UNITY_EXECUTABLE) -manualLicenseFile $(UNITY_LICENSE) -quit -projectPath $(CURDIR) -batchmode -nographics -buildTarget Android -executeMethod LudumDare50.EditorScripts.CustomBuilders.BuildAndroid $(ANDROID_BUILD_DIR)/$(PROJECT_NAME).apk
	@echo -e "\n\n\t/---------------------\\"
	@echo -e "\t Android build finished!"
	@echo -e "\t\\---------------------/\n\n"

--zip_linux_build: $(LINUX_BUILD_DIR)
	@echo -e "\tcreating linux build zip file at $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Linux.zip ...\n"
	@rm -f $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Linux.zip
	@cd $(LINUX_BUILD_DIR) && zip -r ../$(PROJECT_NAME)_$(VERSION)_Linux.zip ./

--zip_windows_build: $(WINDOWS_BUILD_DIR)
	@echo -e "\tcreating windows build zip file at $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Windows.zip ...\n"
	@rm -f $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Windows.zip
	@cd $(WINDOWS_BUILD_DIR) && zip -r ../$(PROJECT_NAME)_$(VERSION)_Windows.zip ./

--zip_web_build: $(WEB_BUILD_DIR)
	@echo -e "\tcreating web build zip file at $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Web.zip ...\n"
	@rm -f $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Web.zip
	@cd $(WEB_BUILD_DIR) && zip -r ../$(PROJECT_NAME)_$(VERSION)_Web.zip ./

--zip_android_build: $(ANDROID_BUILD_DIR)
	@echo -e "\tcreating android build zip file at $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Android.zip ...\n"
	@rm -f $(BUILD_DIR)/$(PROJECT_NAME)_$(VERSION)_Android.zip
	@cd $(ANDROID_BUILD_DIR) && zip -r ../$(PROJECT_NAME)_$(VERSION)_Android.zip ./
