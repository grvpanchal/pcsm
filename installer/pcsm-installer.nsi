#=======
#Basics
#=======
#------------------------------------------------------------------------------------------------------
RequestExecutionLevel user ; << Required, you cannot use admin!
BrandingText "Performance Maintainer Installer"
!define APPNAME "Performance Maintainer"
!define COMPANYNAME "PC Starters"
!define DESCRIPTION "Automated maintenance of disk, registry and Drive"
# These three must be integers
!define VERSIONMAJOR 0.6
!define VERSIONMINOR 0.0
!define VERSIONBUILD 6
!define DOTNET_VERSION "3.5"
# These will be displayed by the "Click here for support information" link in "Add/Remove Programs"
# It is possible to use "mailto:" links in here to open the email client
!define HELPURL "http://sf.net/projects/pcsm/" # "Support Information" link
!define UPDATEURL "http://sf.net/projects/pcsm/" # "Product Updates" link
!define ABOUTURL "http://www.pcstarters.net/authors.php" # "Publisher" link
# This is the size (in kB) of all the files copied into "Program Files"
!define INSTALLSIZE 5000
 
 
InstallDir "$PROGRAMFILES\${COMPANYNAME}\${APPNAME}"
 
# rtf or txt file - remember if it is txt, it must be in the DOS text format (\r\n)
# This will be in the installer/uninstaller's title bar
Name "${APPNAME}"
OutFile "pcsm-setup-${VERSIONMAJOR}.exe"
#------------------------------------------------------------------------------------------------------


#=========
#Includes
#=========
#------------------------------------------------------------------------------------------------------

# NSIS includes
!include MUI2.nsh
!include UAC.nsh
!include DotNET.nsh

#------------------------------------------------------------------------------------------------------


#====
#UAC 
#====
#------------------------------------------------------------------------------------------------------
!macro Init thing
uac_tryagain:
!insertmacro UAC_RunElevated
${Switch} $0
${Case} 0
	${IfThen} $1 = 1 ${|} Quit ${|} ;we are the outer process, the inner process has done its work, we are done
	${IfThen} $3 <> 0 ${|} ${Break} ${|} ;we are admin, let the show go on
	${If} $1 = 3 ;RunAs completed successfully, but with a non-admin user
		MessageBox mb_YesNo|mb_IconExclamation|mb_TopMost|mb_SetForeground "This ${thing} requires admin privileges, try again" /SD IDNO IDYES uac_tryagain IDNO 0
	${EndIf}
	;fall-through and die
${Case} 1223
	MessageBox mb_IconStop|mb_TopMost|mb_SetForeground "This ${thing} requires admin privileges, aborting!"
	Quit
${Case} 1062
	MessageBox mb_IconStop|mb_TopMost|mb_SetForeground "Logon service not running, aborting!"
	Quit
${Default}
	MessageBox mb_IconStop|mb_TopMost|mb_SetForeground "Unable to elevate , error $0"
	Quit
${EndSwitch}
 
SetShellVarContext all
!macroend
 
Function un.onInit
MessageBox MB_OKCANCEL "Permanantly remove ${APPNAME}?" IDOK next
		Abort
	next:
!insertmacro Init "uninstaller"
FunctionEnd
#------------------------------------------------------------------------------------------------------


#=======================
# MUI Installer Options
#=======================
#------------------------------------------------------------------------------------------------------
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "images\installer-top.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "images\installer-side.bmp"
!define MUI_COMPONENTSPAGE_NODESC
Function LaunchLink
  ExecShell "" "$SMPROGRAMS\${COMPANYNAME}\${APPNAME}.lnk"
FunctionEnd
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Run Program"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
!define MUI_FINISHPAGE
!define MUI_ABORTWARNING

# Welcome page
#--------------------------------------------------------------------------------------
!insertmacro MUI_PAGE_WELCOME
#--------------------------------------------------------------------------------------

# License page
#--------------------------------------------------------------------------------------
!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
#--------------------------------------------------------------------------------------

# Components page
#--------------------------------------------------------------------------------------
!insertmacro MUI_PAGE_COMPONENTS
#--------------------------------------------------------------------------------------

#Scheduled page
#--------------------------------------------------------------------------------------

Page Custom pre nsDialogsPageLeave

 
var dialog
 var /GLOBAL schedule
var Group1Radio1
var Group1Radio2
var Group1Radio3
var Group1Radio4
var Group1Radio5

 
Function pre
	!insertmacro MUI_HEADER_TEXT "Schedule" "Scheduled Maintenance"
	nsDialogs::Create 1018
		Pop $dialog
	${NSD_CreateLabel} 0 0 40% 6% "Select Scheduled Maintenance Type:"
	
	${NSD_CreateRadioButton} 0 12% 40% 6% "HOURLY"
		Pop $Group1Radio1
		${NSD_AddStyle} $Group1Radio1 ${WS_GROUP}
		
	${NSD_CreateRadioButton} 0 24% 40% 6% "DAILY"
		Pop $Group1Radio2
		${NSD_Check} $Group1Radio2
	${NSD_CreateRadioButton} 0 36% 40% 6% "WEEKLY"
		Pop $Group1Radio3
		
	${NSD_CreateRadioButton} 0 48% 40% 6% "MONTHLY"
		Pop $Group1Radio4
	
	${NSD_CreateRadioButton} 0 60% 40% 6% "NONE"
		Pop $Group1Radio5
		
 
	nsDialogs::Show
FunctionEnd
 
Function nsDialogsPageLeave
	
	${NSD_GetState} $Group1Radio1 $R1
	
	${If} $R1 == 1
		StrCpy $schedule "hourly"
	${EndIf}
	${NSD_GetState} $Group1Radio2 $R1
	
	${If} $R1 == 1
		StrCpy $schedule "daily"
	${EndIf}
	${NSD_GetState} $Group1Radio3 $R1
	
	${If} $R1 == 1
		StrCpy $schedule "weekly"
		
	${EndIf}
	${NSD_GetState} $Group1Radio4 $R1
	
	${If} $R1 == 1
		StrCpy $schedule "monthly"
	${EndIf}
	${NSD_GetState} $Group1Radio5 $R1
	${If} $R1 == 1
		StrCpy $schedule "none"
	${EndIf}
	
	
FunctionEnd

#--------------------------------------------------------------------------------------

# Directory page
#--------------------------------------------------------------------------------------
!insertmacro MUI_PAGE_DIRECTORY
#--------------------------------------------------------------------------------------

# Run installation
#--------------------------------------------------------------------------------------
!define MUI_PAGE_CUSTOMFUNCTION_SHOW tasbarprogress
!insertmacro MUI_PAGE_INSTFILES 
Function tasbarprogress
w7tbp::Start
FunctionEnd

#--------------------------------------------------------------------------------------

# Finish page
#--------------------------------------------------------------------------------------
!insertmacro MUI_PAGE_FINISH
#--------------------------------------------------------------------------------------

# Uninstaller pages
#--------------------------------------------------------------------------------------
!insertmacro MUI_UNPAGE_INSTFILES
#--------------------------------------------------------------------------------------

# Language files
#--------------------------------------------------------------------------------------
!insertmacro MUI_LANGUAGE "English"
#--------------------------------------------------------------------------------------
#------------------------------------------------------------------------------------------------------


#================
#File Proverties
#================
#------------------------------------------------------------------------------------------------------
!define installerbuild "1"
  VIProductVersion "${VERSIONMAJOR}.0.0"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Performance Maintainer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "${COMPANYNAME}"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" ""
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Performance Maintainer Installer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${VERSIONMAJOR}.0.0"
#------------------------------------------------------------------------------------------------------


#===========
# Variables
#===========
#------------------------------------------------------------------------------------------------------

#------------------------------------------------------------------------------------------------------


#=================
# Install Options
#=================
#------------------------------------------------------------------------------------------------------
ShowInstDetails "nevershow"
#InstType "full"

#------------------------------------------------------------------------------------------------------

 



/*   Section "Microsoft .NET Framework v4.0" dotnet
	Nsisdl::Download http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe $TEMP/dotnet.exe
    #SectionIn RO
    SetOutPath "$TEMP"
    DetailPrint "Installing..."
    Banner::show /NOUNLOAD "Installing..."
    ExecWait '$TEMP\dotnet.exe /q /norestart'   
	DetailPrint "Installing..."	
    Banner::destroy
    SetRebootFlag false
  SectionEnd */

 
Section "Performance Maintainer"
	SectionIn RO 
	!insertmacro CheckDotNET ${DOTNET_VERSION}
	# Files for the install directory - to build the installer, these should be in the same directory as the install script (this file)
	CreateDirectory $INSTDIR
	setOutPath $INSTDIR
	# Files added here should be removed by the uninstaller (see section "uninstall")
	File /r "pcsm\*.*"
	/* CreateDirectory "$INSTDIR\settings\"
	SetOutPath "$INSTDIR\settings\"
	IfFileExists "$INSTDIR\settings\downloadsettings.ini" +3 0
	File "pcsm\settings\*.*" */
	
	
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "checkforupdates" "false"
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "filename" "pcsmnew.exe"
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "link" "http://sourceforge.net/projects/pcsm/files/0.3/pcsm-setup-0.3.exe"
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "schedule" "daily"
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "firstrun" "true"	
	#WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "lastupdatecheck" "05/29/2008"
	
	/* CreateDirectory "$INSTDIR\ud\"
	SetOutPath "$INSTDIR\ud\"
	File "pcsm\ud\*.*"
	CreateDirectory "$INSTDIR\log\"
	CreateDirectory "$INSTDIR\lrc\"
	SetOutPath "$INSTDIR\lrc\"
	File "pcsm\lrc\*.*"
	CreateDirectory "$INSTDIR\blb\"
	SetOutPath "$INSTDIR\blb\"
	File "pcsm\blb\*.*"
	CreateDirectory "$INSTDIR\lro\"
	SetOutPath "$INSTDIR\lro\"
	File "pcsm\lro\*.*"
	CreateDirectory "$INSTDIR\svcopz\"
	SetOutPath "$INSTDIR\svcopz\"
	File "pcsm\svcopz\*.*"
	CreateDirectory "$INSTDIR\lib\"
	SetOutPath "$INSTDIR\lib\"
	File "pcsm\lib\*.*" */
	# Add any other files for the install directory (license files, app data, etc) here
	SetOutPath "$INSTDIR"
	# Uninstaller - See function un.onInit and section "uninstall" for configuration
	writeUninstaller "$INSTDIR\uninstall.exe"
 
	# Start Menu
	setShellVarContext all
	CreateDirectory "$SMPROGRAMS\${COMPANYNAME}"
	CreateShortCut "$SMPROGRAMS\${COMPANYNAME}\${APPNAME}.lnk" "$INSTDIR\pcsm.exe"  
	CreateShortCut "$SMPROGRAMS\${COMPANYNAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe" 
 
	# Registry information for add/remove programs
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayIcon" "$\"$INSTDIR\icon.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "Publisher" "${COMPANYNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "HelpLink" "$\"${HELPURL}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayVersion" "${VERSIONMAJOR}"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMajor" ${VERSIONMAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "VersionMinor" ${VERSIONMINOR}
	# There is no option for modifying or repairing the install
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoRepair" 1
	# Set the INSTALLSIZE constant (!defined at the top of this script) so Add/Remove Programs can accurately report the size
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "EstimatedSize" ${INSTALLSIZE}
	# Startup Manager Fix
	WriteRegDWORD HKLM "Software\Startup Manager" "abc" 0xDEADBEE
	SetRegView 64
    WriteRegDWORD HKLM "Software\Startup Manager" "abc" 0xDEADBEE
    SetRegView 32
SectionEnd

/* Section "Check for Updates"
	WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "checkforupdates" "true"
SectionEnd */


Section "" 
	CreateDirectory "$WINDIR\pcsm\"
	CreateDirectory "$PROGRAMFILES\Common Files\Little Registry Cleaner\Logs"
	SetOutPath "$WINDIR\pcsm\"
	File "pcsmwin\*.*"
	File "pcsm\pcsm.exe"
	File "pcsm\pcsm.exe.config"
	File "pcsm\lib\Newtonsoft.Json.dll"	
	File "pcsm\lib\System.Windows.Forms.DataVisualization.dll"	
	CreateDirectory "$WINDIR\pcsm\log"
	CreateDirectory "$WINDIR\pcsm\ud\"
	SetOutPath "$WINDIR\pcsm\ud\"
	File "pcsm\ud\*.*"
	CreateDirectory "$WINDIR\pcsm\lrc\"
	SetOutPath "$WINDIR\pcsm\lrc\"
	File /r "pcsm\lrc\*.*"
	CreateDirectory "$WINDIR\pcsm\blb\"
	SetOutPath "$WINDIR\pcsm\blb\"
	File /r "pcsm\blb\*.*"	
	File /r "pcsmwin\blb\*.*"	
	CreateDirectory "$WINDIR\pcsm\settings\"
	SetOutPath "$WINDIR\pcsm\settings\"
	File "pcsmwin\settings\*.*"
	ExecShell "open" "schtasks" " /DELETE /TN $\"Performance Maintainer$\" /F" "SW_HIDE"
	Sleep 5000
${If} $schedule == "hourly"
ExecShell "open" "$INSTDIR\scht.exe" " /hourly" "SW_HIDE"
${ElseIf} $schedule == "daily"
ExecShell "open" "$INSTDIR\scht.exe" " /daily" "SW_HIDE"
${ElseIf} $schedule == "weekly"
ExecShell "open" "$INSTDIR\scht.exe" " /weekly" "SW_HIDE"
${ElseIf} $schedule == "monthly"
ExecShell "open" "$INSTDIR\scht.exe" " /monthly" "SW_HIDE"
/* ${ElseIf} $schedule == "none"
WriteINIStr "$INSTDIR\settings\downloadsettings.ini" "main" "schedule" "none" */
${Else}
${EndIf}
ExecShell "open" "$INSTDIR\pcsm\blb\bleachbit_console.exe" " --update-winapp2" "SW_HIDE"

SectionEnd


Function .onInit
	setShellVarContext all
	 !insertmacro Init "installer"
	/*ReadRegStr $1 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{3C3901C5-3455-3E0A-A214-0B093A5070A6}" "UninstallString"	
	${If} $1 != ""	
		SectionSetFlags ${dotnet} 0
		SectionSetFlags ${dotnet} 16
		SectionSetText ${dotnet} "Microsoft .NET Framework v4.0 Installed" 
	${EndIf}	
	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F5B09CFD-F0B2-36AF-8DF4-1DF6B63FC7B4}" "UninstallString"	
	${If} $1 != ""	
		SectionSetFlags ${dotnet} 0
		SectionSetFlags ${dotnet} 16
		SectionSetText ${dotnet} "Microsoft .NET Framework v4.0 Installed" 
	${EndIf}
	ReadRegStr $1 HKLM "SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v4.0" ""
		
	${If} $1 != ""	
		SectionSetFlags ${dotnet} 0
		SectionSetFlags ${dotnet} 16
		SectionSetText ${dotnet} "Microsoft .NET Framework v4.0 Installed" 
	${EndIf} */
FunctionEnd

# Uninstaller
Section "uninstall"
	
	ExecShell "open" "taskkill" "/im pcsm.exe  /im lrc.exe /im dclean.exe /im udefrag.exe /T /F"
	
	ExecShell "open" "schtasks" " /DELETE /TN $\"Performance Maintainer$\" /F" "SW_HIDE"
	
	ExecShell "open" "Disk Cleaner\uninstall.exe" " /S" "SW_HIDE"
	Sleep 5000
	ExecShell "open" "Startup Manager\unins000.exe" " /VERYSILENT /SUPPRESSMSGBOXES" "SW_HIDE"
	Sleep 5000
	RMDir /r "$WINDIR\pcsm\"
	
	RMDir /r $INSTDIR
	setShellVarContext all
	RMDir /r "$SMPROGRAMS\${COMPANYNAME}"
	setShellVarContext current
	RMDir /r "$SMPROGRAMS\${COMPANYNAME}"
 
	# Remove uninstaller information from the registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}"
	
SectionEnd




 

 