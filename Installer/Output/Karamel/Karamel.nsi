############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

!define APP_NAME "Karamel"
!define COMP_NAME "Akrostichon Karaoke"
!define WEB_SITE "http://sourceforge.net/projects/karamel/"
!define VERSION "01.02.00.00"
!define COPYRIGHT "Dominik Damerow © 2015"
!define DESCRIPTION "The simple Karaoke Player"
!define LICENSE_TXT "F:\Karamel\Libs\License.txt"
!define INSTALLER_NAME "F:\Karamel\Installer\Output\Karamel\Karamel_v1.02.exe"
!define MAIN_APP_EXE "Karamel.exe"
!define INSTALL_TYPE "SetShellVarContext current"
!define REG_ROOT "HKCU"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

!define REG_START_MENU "Start Menu Folder"

var SM_Folder

######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor ZLIB
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "$PROGRAMFILES\Karamel"

######################################################################

!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME

!ifdef LICENSE_TXT
!insertmacro MUI_PAGE_LICENSE "${LICENSE_TXT}"
!endif

!insertmacro MUI_PAGE_DIRECTORY

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "Karamel"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\${MAIN_APP_EXE}"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

######################################################################

Section -MainProgram
${INSTALL_TYPE}
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
File "F:\Karamel\Runtime\Business.dll"
File "F:\Karamel\Runtime\Business.pdb"
File "F:\Karamel\Runtime\DatabaseConnection.dll"
File "F:\Karamel\Runtime\DatabaseConnection.pdb"
File "F:\Karamel\Runtime\DataGridFilterLibrary.dll"
File "F:\Karamel\Runtime\GongSolutions.Wpf.DragDrop.dll"
File "F:\Karamel\Runtime\Ionic.Zip.dll"
File "F:\Karamel\Runtime\Karamel.exe"
File "F:\Karamel\Runtime\Karamel.exe.config"
File "F:\Karamel\Runtime\Karamel.Infrastructure.dll"
File "F:\Karamel\Runtime\Karamel.Infrastructure.dll.config"
File "F:\Karamel\Runtime\Karamel.Infrastructure.pdb"
File "F:\Karamel\Runtime\Karamel.pdb"
File "F:\Karamel\Runtime\Karamel.vshost.exe"
File "F:\Karamel\Runtime\Karamel.vshost.exe.config"
File "F:\Karamel\Runtime\Karamel.vshost.exe.manifest"
File "F:\Karamel\Runtime\KCdgPlayer.dll"
File "F:\Karamel\Runtime\MediaLibrary.dll"
File "F:\Karamel\Runtime\MediaLibrary.dll.config"
File "F:\Karamel\Runtime\MediaLibrary.pdb"
File "F:\Karamel\Runtime\MediaPlayer.dll"
File "F:\Karamel\Runtime\MediaPlayer.dll.config"
File "F:\Karamel\Runtime\MediaPlayer.pdb"
File "F:\Karamel\Runtime\Microsoft.Practices.Prism.dll"
File "F:\Karamel\Runtime\Microsoft.Practices.Prism.UnityExtensions.dll"
File "F:\Karamel\Runtime\Microsoft.Practices.ServiceLocation.dll"
File "F:\Karamel\Runtime\Microsoft.Practices.Unity.dll"
File "F:\Karamel\Runtime\PlayerControl.dll"
File "F:\Karamel\Runtime\PlayerControl.pdb"
File "F:\Karamel\Runtime\Playlist.dll"
File "F:\Karamel\Runtime\Playlist.pdb"
File "F:\Karamel\Runtime\SingerLibrary.dll"
File "F:\Karamel\Runtime\SingerLibrary.pdb"
File "F:\Karamel\Runtime\taglib-sharp.dll"
SectionEnd

######################################################################

Section -Icons_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstall.exe"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"

!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!insertmacro MUI_STARTMENU_WRITE_END
!endif

!ifndef REG_START_MENU
CreateDirectory "$SMPROGRAMS\Karamel"
CreateShortCut "$SMPROGRAMS\Karamel\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\Karamel\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"

!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\Karamel\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!endif

WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

!ifdef WEB_SITE
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
!endif
SectionEnd

######################################################################

Section Uninstall
${INSTALL_TYPE}
Delete "$INSTDIR\Business.dll"
Delete "$INSTDIR\Business.pdb"
Delete "$INSTDIR\DatabaseConnection.dll"
Delete "$INSTDIR\DatabaseConnection.pdb"
Delete "$INSTDIR\DataGridFilterLibrary.dll"
Delete "$INSTDIR\GongSolutions.Wpf.DragDrop.dll"
Delete "$INSTDIR\Ionic.Zip.dll"
Delete "$INSTDIR\Karamel.exe"
Delete "$INSTDIR\Karamel.exe.config"
Delete "$INSTDIR\Karamel.Infrastructure.dll"
Delete "$INSTDIR\Karamel.Infrastructure.dll.config"
Delete "$INSTDIR\Karamel.Infrastructure.pdb"
Delete "$INSTDIR\Karamel.pdb"
Delete "$INSTDIR\Karamel.vshost.exe"
Delete "$INSTDIR\Karamel.vshost.exe.config"
Delete "$INSTDIR\Karamel.vshost.exe.manifest"
Delete "$INSTDIR\KCdgPlayer.dll"
Delete "$INSTDIR\MediaLibrary.dll"
Delete "$INSTDIR\MediaLibrary.dll.config"
Delete "$INSTDIR\MediaLibrary.pdb"
Delete "$INSTDIR\MediaPlayer.dll"
Delete "$INSTDIR\MediaPlayer.dll.config"
Delete "$INSTDIR\MediaPlayer.pdb"
Delete "$INSTDIR\Microsoft.Practices.Prism.dll"
Delete "$INSTDIR\Microsoft.Practices.Prism.UnityExtensions.dll"
Delete "$INSTDIR\Microsoft.Practices.ServiceLocation.dll"
Delete "$INSTDIR\Microsoft.Practices.Unity.dll"
Delete "$INSTDIR\PlayerControl.dll"
Delete "$INSTDIR\PlayerControl.pdb"
Delete "$INSTDIR\Playlist.dll"
Delete "$INSTDIR\Playlist.pdb"
Delete "$INSTDIR\SingerLibrary.dll"
Delete "$INSTDIR\SingerLibrary.pdb"
Delete "$INSTDIR\taglib-sharp.dll"
Delete "$INSTDIR\uninstall.exe"
!ifdef WEB_SITE
Delete "$INSTDIR\${APP_NAME} website.url"
!endif

RmDir "$INSTDIR"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
!endif
RmDir "$SMPROGRAMS\$SM_Folder"
!endif

!ifndef REG_START_MENU
Delete "$SMPROGRAMS\Karamel\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\Karamel\Uninstall ${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\Karamel\${APP_NAME} Website.lnk"
!endif
RmDir "$SMPROGRAMS\Karamel"
!endif

DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################

