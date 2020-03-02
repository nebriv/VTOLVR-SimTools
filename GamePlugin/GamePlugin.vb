﻿Option Explicit On
Option Strict On

Imports Game_PluginAPI
Imports System.IO

Public Class GamePlugin
    Implements IPlugin_Game
    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///                            SimTools Plugin - Edit the Setting below to provide support for your favorite game!                             ///
    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '//////////////////////////////////////////////// 
    '/// Per Game Settings - Change for Each Game ///
    '////////////////////////////////////////////////
    Private Const _PluginAuthorsName As String = "nebriv"
    Private Const _GameName As String = "VTOLVR" 'GameName (Must Be Unique!) - the displayed Name.
    Private Const _ProcessName As String = "vtolvr" 'Process_Name without the (".exe") for this game
    Private Const _Port As String = "4123"  'Your Sending/Recieving UDP Port for this game
    Private Const _RequiresPatchingPath As Boolean = False 'do we need the game exe path for patching? (must be True if _RequiresSecondCheck = True)
    Private Const _RequiresSecondCheck As Boolean = False 'Used when games have the same _ProcessName. (all plugins with the same _ProcessName must use this!)
    Private Const _PluginOptions As String = "" 'Reserved For Future Use - No Change Needed 
    '////////////////////////////////////////////////
    '///           Memory Map Variables           ///
    '////////////////////////////////////////////////
    Private Const _Enable_MemoryMap As Boolean = False 'Is a MemoryMap file Required for this game?
    Private Const _MMF_Name As String = "NULL" 'Set if using a Memory Map File - EXAMPLE("$gtr2$")
    '////////////////////////////////////////////////
    '///           MemoryHook Variables           ///
    '////////////////////////////////////////////////   
    Private Const _Enable_MemoryHook As Boolean = False 'Is a Memory Hook Required for this game? 
    Private Const _MemHook_Roll As UInteger = 0 'Not Used = 0
    Private Const _MemHook_Pitch As UInteger = 0
    Private Const _MemHook_Heave As UInteger = 0
    Private Const _MemHook_Yaw As UInteger = 0
    Private Const _MemHook_Sway As UInteger = 0
    Private Const _MemHook_Surge As UInteger = 0
    Private Const _MemHook_Extra1 As UInteger = 0
    Private Const _MemHook_Extra2 As UInteger = 0
    Private Const _MemHook_Extra3 As UInteger = 0
    '////////////////////////////////////////////////
    '///    DOFs Used for Output for this Game    ///
    '////////////////////////////////////////////////
    Private Const _DOF_Support_Roll As Boolean = True
    Private Const _DOF_Support_Pitch As Boolean = True
    Private Const _DOF_Support_Heave As Boolean = True
    Private Const _DOF_Support_Yaw As Boolean = True
    Private Const _DOF_Support_Sway As Boolean = True
    Private Const _DOF_Support_Surge As Boolean = True
    Private Const _DOF_Support_Extra1 As String = "" 'Blank = False
    Private Const _DOF_Support_Extra2 As String = "" '"" = Not Used
    Private Const _DOF_Support_Extra3 As String = "" 'ADD THE FORCE NAME HERE
    '/////////////////////////////////////////////////
    '///       GameDash - Dash Board Support       ///
    '/////////////////////////////////////////////////
    Private Const _Enable_DashBoard As Boolean = False  'Enable the DashBoard Output System?
    'EXAMPLES OF DASH OUTPUT (all variables are strings) - 20 DASH OUTPUTS MAX!!!
    'Use the Variables with either Process_PacketRecieved, Process_MemoryHook or Process_MemoryMap.
    'Variable = (*Action, Value) as String - (The *Action variable can be anything)
    'Basic Dash Support should include at the minimum -  SPEED, RPM and GEAR
    'Dash_1_Output = "Speed," & OUTPUT_VALUE_HERE.ToString
    'Dash_2_Output = "Rpm," & OUTPUT_VALUE_HERE.ToString
    'Dash_2_Output = "Gear," & OUTPUT_VALUE_HERE.ToString
    '...
    'Dash_20_Output = "Engine Temp," & OUTPUT_VALUE_HERE.ToString
    '/////////////////////////////////////////////////
    '///           GameVibe Support              ///
    '/////////////////////////////////////////////////
    Private Const _Enable_GameVibe As Boolean = False 'Enable the GameVibe Output System?
    'EXAMPLES OF GameVibe OUTPUT (all variables are strings) - 9 GameVibe OUTPUTS MAX!!!
    'Use the Variables with either Process_PacketRecieved, Process_MemoryHook or Process_MemoryMap.
    'Variable = (*Action, Value) as String - (The *Action" variable can ONLY be: Rpm, Heave, Sway, Surge, FR, FL, RR, RL, Turbo)
    'Basic GameVibe Support should include at the minimum -  Rpm
    'Vibe_1_Output = "Rpm," & OUTPUT_VALUE_HERE.ToString
    'Vibe_2_Output = "Heave," & OUTPUT_VALUE_HERE.ToString
    'Vibe_3_Output = "Sway," & OUTPUT_VALUE_HERE.ToString
    'Vibe_4_Output = "Surge," & OUTPUT_VALUE_HERE.ToString
    '...
    'Vibe_9_Output = "Gear Shift," & OUTPUT_VALUE_HERE.ToString
    '/////////////////////////////////////////////////

    'Used by GameManager when the Game Starts.
    Public Sub GameStart() Implements IPlugin_Game.GameStart
        'DO SOMETHING HERE AT GAME START!
    End Sub

    'Used by GameManager when the Game Stops.
    Public Sub GameStop() Implements IPlugin_Game.GameEnd
        'DO SOMETHING HERE AT GAME STOP!
    End Sub

    'Used by GameManager to Process a MemoryHook.
    Public Sub Process_MemoryHook() Implements IPlugin_Game.Process_MemoryHook
        'DO SOMETHING HERE AT GAME START!        
    End Sub

    'Used by GameManager to Process a MemoryMap.
    Public Sub Process_MemoryMap() Implements IPlugin_Game.Process_MemoryMap
        'DO SOMETHING HERE AT GAME START!
    End Sub

    'Used by GameEngine to Process Incoming UDP Packets.
    Public Sub Process_PacketRecieved(received_data As String) Implements IPlugin_Game.Process_PacketRecieved

        If received_data.StartsWith("S:") AndAlso received_data.EndsWith(":E") Then
            Dim spearator As String() = {":"}
            Dim count As Int32 = 10
            Dim strlist As String() = received_data.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries)
            Yaw_Output = CDbl(strlist(1)) 'Yaw
            Pitch_Output = CDbl(strlist(2)) 'Pitch
            Roll_Output = CDbl(strlist(3)) 'Roll
            Surge_Output = CDbl(strlist(4)) 'X
            Sway_Output = CDbl(strlist(5)) 'Y
            Heave_Output = CDbl(strlist(6)) 'Z
        End If

    End Sub

    'Used by GameManager to Patch a Game.
    Public Function PatchGame(ByVal MyPath As String, ByVal MyIp As String) As Boolean Implements IPlugin_Game.PatchGame
        'Change as Needed
        'If game is already patched - Unpatch first
        'Change as Needed
        'If game is already patched - Unpatch first
        UnPatch(MyPath)

        Try
            'System.IO.File.Copy(MyPath & "\hardwaresettings\hardware_settings_config.xml", MyPath & "\hardwaresettings\cfg.BAK")
            'SetAttr(MyPath & "\hardwaresettings\hardware_settings_config.xml", GetAttr(MyPath & "\hardwaresettings\hardware_settings_config.xml") And (Not vbReadOnly))
            'MyFileEditor(MyPath & "\hardwaresettings\hardware_settings_config.xml", "<motion enabled=", "<motion enabled=" & Chr(34) & "true" & Chr(34) & " ip=" & Chr(34) & MyIp & Chr(34) & " port=" & Chr(34) & "4123" & Chr(34) & " delay=" & Chr(34) & "1" & Chr(34) & " extradata=" & Chr(34) & "0" & Chr(34) & " />")
            'MsgBox("Patch Installed!")
            Return True
        Catch ex As Exception
            'MsgBox("You Must first Install and Run the Game Once Prior to Patching" & vbCrLf & "You may also add Custom Game Installation Directories below!")
            Return False
        End Try

    End Function

    'Used by GameManager to UnPatch a Game.
    Public Sub UnPatchGame(MyPath As String) Implements IPlugin_Game.UnPatchGame
        'Change as Needed
        UnPatch(MyPath)
        'MsgBox("Patch Uninstalled!", MsgBoxStyle.OkOnly, "Patching info")
    End Sub

    'Used by GameManager to UnPatch a Game.
    Private Sub UnPatch(MyPath As String)

    End Sub

    'Tells the User where to patch the game 
    Public Sub PatchPathInfo() Implements IPlugin_Game.PatchPathInfo
        'Tell the User where to patch the game 
    End Sub

    'Used by GameManager to Validate a Path befors Patching.
    Public Function ValidatePatchPath(MyPath As String) As Boolean Implements IPlugin_Game.ValidatePatchPath
        'Tell the User where to patch the game 
        Return True
    End Function


    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///                                                 PLACE EXTRA NEEDED CODE/FUNCTIONS HERE                                                     ///
    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#Region "///// - EXTRA CODE/FUNCTIONS USED FOR THIS PLUGIN - /////"
    'Used by GameEngine to Process Incoming UDP Packets.
    Private MyOutsim_Internal As New OutSim
    Private Structure OutSim
        '   time in milliseconds
        Public bytTime1 As Byte
        Public bytTime2 As Byte
        Public bytTime3 As Byte
        Public bytTime4 As Byte
        '   Angular Velocity
        Public sngAVelocity0 As Single
        Public sngAVelocity1 As Single
        Public sngAVelocity2 As Single
        '   Orientation
        Public sngOrientation0 As Single
        Public sngOrientation1 As Single
        Public sngOrientation2 As Single
        '   Acceleration 
        Public sngAcceleration0 As Single
        Public sngAcceleration1 As Single
        Public sngAcceleration2 As Single
        '   Velocity
        Public sngVelocity0 As Single
        Public sngVelocity1 As Single
        Public sngVelocity2 As Single
        '   Position
        Public bytPosition01 As Byte
        Public bytPosition02 As Byte
        Public bytPosition03 As Byte
        Public bytPosition04 As Byte
        Public bytPosition11 As Byte
        Public bytPosition12 As Byte
        Public bytPosition13 As Byte
        Public bytPosition14 As Byte
        Public bytPosition21 As Byte
        Public bytPosition22 As Byte
        Public bytPosition23 As Byte
        Public bytPosition24 As Byte
        '   Game ID - if specified in cfg.txt
        Public lngGameID As Long
    End Structure

    'File Editor - Replaces a line in a txt/cfg file
    Private Sub MyFileEditor(FileName As String, LinetoEdit As String, StingReplacment As String)
        Dim lines As New List(Of String)
        Dim x As Integer = 0
        Using sr As New System.IO.StreamReader(FileName)
            While Not sr.EndOfStream
                lines.Add(sr.ReadLine)
            End While
        End Using

        For Each line As String In lines
            If line.Contains(LinetoEdit) Then
                lines.RemoveAt(x)
                lines.Insert(x, StingReplacment)
                Exit For 'must exit as we changed the iteration   
            End If
            x = x + 1
        Next

        Using sw As New System.IO.StreamWriter(FileName)
            For Each line As String In lines
                sw.WriteLine(line)
            Next
        End Using
        lines.Clear()
    End Sub
#End Region


    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '///                                                        DO NOT EDIT BELOW HERE!!!                                                           ///
    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#Region "///// BUILT IN METHODS FOR SIMTOOLS - DO NOT CHANGE /////"
    'Output Dash Vars
    Public Dash_1_Output As String = ""
    Public Dash_2_Output As String = ""
    Public Dash_3_Output As String = ""
    Public Dash_4_Output As String = ""
    Public Dash_5_Output As String = ""
    Public Dash_6_Output As String = ""
    Public Dash_7_Output As String = ""
    Public Dash_8_Output As String = ""
    Public Dash_9_Output As String = ""
    Public Dash_10_Output As String = ""
    Public Dash_11_Output As String = ""
    Public Dash_12_Output As String = ""
    Public Dash_13_Output As String = ""
    Public Dash_14_Output As String = ""
    Public Dash_15_Output As String = ""
    Public Dash_16_Output As String = ""
    Public Dash_17_Output As String = ""
    Public Dash_18_Output As String = ""
    Public Dash_19_Output As String = ""
    Public Dash_20_Output As String = ""

    'Output Vars
    Public Roll_Output As Double = 0
    Public Pitch_Output As Double = 0
    Public Heave_Output As Double = 0
    Public Yaw_Output As Double = 0
    Public Sway_Output As Double = 0
    Public Surge_Output As Double = 0
    Public Extra1_Output As Double = 0
    Public Extra2_Output As Double = 0
    Public Extra3_Output As Double = 0

    'MemHook Vars
    Public Roll_MemHook As Double = 0
    Public Pitch_MemHook As Double = 0
    Public Heave_MemHook As Double = 0
    Public Yaw_MemHook As Double = 0
    Public Sway_MemHook As Double = 0
    Public Surge_MemHook As Double = 0
    Public Extra1_MemHook As Double = 0
    Public Extra2_MemHook As Double = 0
    Public Extra3_MemHook As Double = 0

    'MemMap Vars
    Public Roll_MemMap As Double = 0
    Public Pitch_MemMap As Double = 0
    Public Heave_MemMap As Double = 0
    Public Yaw_MemMap As Double = 0
    Public Sway_MemMap As Double = 0
    Public Surge_MemMap As Double = 0
    Public Extra1_MemMap As Double = 0
    Public Extra2_MemMap As Double = 0
    Public Extra3_MemMap As Double = 0

    'GameVibe Vars
    Public Vibe_1_Output As String = ""
    Public Vibe_2_Output As String = ""
    Public Vibe_3_Output As String = ""
    Public Vibe_4_Output As String = ""
    Public Vibe_5_Output As String = ""
    Public Vibe_6_Output As String = ""
    Public Vibe_7_Output As String = ""
    Public Vibe_8_Output As String = ""
    Public Vibe_9_Output As String = ""

    Public Function Get_PluginVersion() As String Implements IPlugin_Game.Get_PluginVersion
        Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
    End Function

    Public Function GetDOFsUsed() As String Implements IPlugin_Game.GetDOFsUsed
        'Return DOF's Used (Roll,Pitch,Heave,Yaw,Sway,Surge)
        Return (_DOF_Support_Roll.ToString & "," & _DOF_Support_Pitch.ToString & "," & _DOF_Support_Heave.ToString & "," & _DOF_Support_Yaw.ToString & "," & _DOF_Support_Sway.ToString & "," & _DOF_Support_Surge.ToString & "," & _DOF_Support_Extra1.ToString & "," & _DOF_Support_Extra2.ToString & "," & _DOF_Support_Extra3.ToString)
    End Function

    Public Sub ResetDOFVars() Implements IPlugin_Game.ResetDOFVars
        Roll_Output = 0
        Pitch_Output = 0
        Heave_Output = 0
        Yaw_Output = 0
        Sway_Output = 0
        Surge_Output = 0
        Extra1_Output = 0
        Extra2_Output = 0
        Extra3_Output = 0
    End Sub

    Public Sub ResetMapVars() Implements IPlugin_Game.ResetMapVars
        Roll_MemMap = 0
        Pitch_MemMap = 0
        Heave_MemMap = 0
        Yaw_MemMap = 0
        Sway_MemMap = 0
        Surge_MemMap = 0
        Extra1_MemMap = 0
        Extra2_MemMap = 0
        Extra3_MemMap = 0
    End Sub

    Public Sub ResetHookVars() Implements IPlugin_Game.ResetHookVars
        Roll_MemHook = 0
        Pitch_MemHook = 0
        Heave_MemHook = 0
        Yaw_MemHook = 0
        Sway_MemHook = 0
        Surge_MemHook = 0
        Extra1_MemHook = 0
        Extra2_MemHook = 0
        Extra3_MemHook = 0
    End Sub

    Public Sub ResetDashVars() Implements IPlugin_Game.ResetDashVars
        Dash_1_Output = ""
        Dash_2_Output = ""
        Dash_3_Output = ""
        Dash_4_Output = ""
        Dash_5_Output = ""
        Dash_6_Output = ""
        Dash_7_Output = ""
        Dash_8_Output = ""
        Dash_9_Output = ""
        Dash_10_Output = ""
        Dash_11_Output = ""
        Dash_12_Output = ""
        Dash_13_Output = ""
        Dash_14_Output = ""
        Dash_15_Output = ""
        Dash_16_Output = ""
        Dash_17_Output = ""
        Dash_18_Output = ""
        Dash_19_Output = ""
        Dash_20_Output = ""
    End Sub

    Public Sub ResetVibeVars() Implements IPlugin_Game.ResetVibeVars
        Vibe_1_Output = ""
        Vibe_2_Output = ""
        Vibe_3_Output = ""
        Vibe_4_Output = ""
        Vibe_5_Output = ""
        Vibe_6_Output = ""
        Vibe_7_Output = ""
        Vibe_8_Output = ""
        Vibe_9_Output = ""
    End Sub

    Public ReadOnly Property PluginAuthorsName() As String Implements IPlugin_Game.PluginAuthorsName
        Get
            Return _PluginAuthorsName
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IPlugin_Game.GameName
        Get
            Return _GameName
        End Get
    End Property

    Public ReadOnly Property ProcessName() As String Implements IPlugin_Game.ProcessName
        Get
            Return _ProcessName
        End Get
    End Property

    Public ReadOnly Property Port() As String Implements IPlugin_Game.Port
        Get
            Return _Port
        End Get
    End Property

    Public ReadOnly Property Enable_MemoryMap() As Boolean Implements IPlugin_Game.Enable_MemoryMap
        Get
            Return _Enable_MemoryMap
        End Get
    End Property

    Public ReadOnly Property Enable_MemoryHook() As Boolean Implements IPlugin_Game.Enable_MemoryHook
        Get
            Return _Enable_MemoryHook
        End Get
    End Property

    Public ReadOnly Property RequiresPatchingPath() As Boolean Implements IPlugin_Game.RequiresPatchingPath
        Get
            Return _RequiresPatchingPath
        End Get
    End Property

    Public ReadOnly Property RequiresSecondCheck() As Boolean Implements IPlugin_Game.RequiresSecondCheck
        Get
            Return _RequiresSecondCheck
        End Get
    End Property

    Public ReadOnly Property Enable_DashBoard() As Boolean Implements IPlugin_Game.Enable_DashBoard
        Get
            Return _Enable_DashBoard
        End Get
    End Property

    Public ReadOnly Property Enable_GameVibe() As Boolean Implements IPlugin_Game.Enable_GameVibe
        Get
            Return _Enable_GameVibe
        End Get
    End Property

    Public Function Get_RollOutput() As Double Implements IPlugin_Game.Get_RollOutput
        Return Roll_Output
    End Function

    Public Function Get_PitchOutput() As Double Implements IPlugin_Game.Get_PitchOutput
        Return Pitch_Output
    End Function

    Public Function Get_HeaveOutput() As Double Implements IPlugin_Game.Get_HeaveOutput
        Return Heave_Output
    End Function

    Public Function Get_YawOutput() As Double Implements IPlugin_Game.Get_YawOutput
        Return Yaw_Output
    End Function

    Public Function Get_SwayOutput() As Double Implements IPlugin_Game.Get_SwayOutput
        Return Sway_Output
    End Function

    Public Function Get_SurgeOutput() As Double Implements IPlugin_Game.Get_SurgeOutput
        Return Surge_Output
    End Function

    Public Function Get_Extra1Output() As Double Implements IPlugin_Game.Get_Extra1Output
        Return Extra1_Output
    End Function

    Public Function Get_Extra2Output() As Double Implements IPlugin_Game.Get_Extra2Output
        Return Extra2_Output
    End Function

    Public Function Get_Extra3Output() As Double Implements IPlugin_Game.Get_Extra3Output
        Return Extra3_Output
    End Function

    Public Function Get_Dash_1_Output() As String Implements IPlugin_Game.Get_Dash1_Output
        Return Dash_1_Output
    End Function

    Public Function Get_Dash_2_Output() As String Implements IPlugin_Game.Get_Dash2_Output
        Return Dash_2_Output
    End Function

    Public Function Get_Dash_3_Output() As String Implements IPlugin_Game.Get_Dash3_Output
        Return Dash_3_Output
    End Function

    Public Function Get_Dash_4_Output() As String Implements IPlugin_Game.Get_Dash4_Output
        Return Dash_4_Output
    End Function

    Public Function Get_Dash_5_Output() As String Implements IPlugin_Game.Get_Dash5_Output
        Return Dash_5_Output
    End Function

    Public Function Get_Dash_6_Output() As String Implements IPlugin_Game.Get_Dash6_Output
        Return Dash_6_Output
    End Function

    Public Function Get_Dash_7_Output() As String Implements IPlugin_Game.Get_Dash7_Output
        Return Dash_7_Output
    End Function

    Public Function Get_Dash_8_Output() As String Implements IPlugin_Game.Get_Dash8_Output
        Return Dash_8_Output
    End Function

    Public Function Get_Dash_9_Output() As String Implements IPlugin_Game.Get_Dash9_Output
        Return Dash_9_Output
    End Function

    Public Function Get_Dash_10_Output() As String Implements IPlugin_Game.Get_Dash10_Output
        Return Dash_10_Output
    End Function

    Public Function Get_Dash_11_Output() As String Implements IPlugin_Game.Get_Dash11_Output
        Return Dash_11_Output
    End Function

    Public Function Get_Dash_12_Output() As String Implements IPlugin_Game.Get_Dash12_Output
        Return Dash_12_Output
    End Function

    Public Function Get_Dash_13_Output() As String Implements IPlugin_Game.Get_Dash13_Output
        Return Dash_13_Output
    End Function

    Public Function Get_Dash_14_Output() As String Implements IPlugin_Game.Get_Dash14_Output
        Return Dash_14_Output
    End Function

    Public Function Get_Dash_15_Output() As String Implements IPlugin_Game.Get_Dash15_Output
        Return Dash_15_Output
    End Function

    Public Function Get_Dash_16_Output() As String Implements IPlugin_Game.Get_Dash16_Output
        Return Dash_16_Output
    End Function

    Public Function Get_Dash_17_Output() As String Implements IPlugin_Game.Get_Dash17_Output
        Return Dash_17_Output
    End Function

    Public Function Get_Dash_18_Output() As String Implements IPlugin_Game.Get_Dash18_Output
        Return Dash_18_Output
    End Function

    Public Function Get_Dash_19_Output() As String Implements IPlugin_Game.Get_Dash19_Output
        Return Dash_19_Output
    End Function

    Public Function Get_Dash_20_Output() As String Implements IPlugin_Game.Get_Dash20_Output
        Return Dash_20_Output
    End Function

    Public Function Get_RollMemHook() As Double Implements IPlugin_Game.Get_RollMemHook
        Return Roll_MemHook
    End Function

    Public Function Get_PitchMemHook() As Double Implements IPlugin_Game.Get_PitchMemHook
        Return Pitch_MemHook
    End Function

    Public Function Get_HeaveMemHook() As Double Implements IPlugin_Game.Get_HeaveMemHook
        Return Heave_MemHook
    End Function

    Public Function Get_YawMemHook() As Double Implements IPlugin_Game.Get_YawMemHook
        Return Yaw_MemHook
    End Function

    Public Function Get_SwayMemHook() As Double Implements IPlugin_Game.Get_SwayMemHook
        Return Sway_MemHook
    End Function

    Public Function Get_SurgeMemHook() As Double Implements IPlugin_Game.Get_SurgeMemHook
        Return Surge_MemHook
    End Function

    Public Function Get_Extra1MemHook() As Double Implements IPlugin_Game.Get_Extra1MemHook
        Return Extra1_MemHook
    End Function

    Public Function Get_Extra2MemHook() As Double Implements IPlugin_Game.Get_Extra2MemHook
        Return Extra2_MemHook
    End Function

    Public Function Get_Extra3MemHook() As Double Implements IPlugin_Game.Get_Extra3MemHook
        Return Extra3_MemHook
    End Function

    Public Function Get_RollMemMap() As Double Implements IPlugin_Game.Get_RollMemMap
        Return Roll_MemMap
    End Function

    Public Function Get_PitchMemMap() As Double Implements IPlugin_Game.Get_PitchMemMap
        Return Pitch_MemMap
    End Function

    Public Function Get_HeaveMemMap() As Double Implements IPlugin_Game.Get_HeaveMemMap
        Return Heave_MemMap
    End Function

    Public Function Get_YawMemMap() As Double Implements IPlugin_Game.Get_YawMemMap
        Return Yaw_MemMap
    End Function

    Public Function Get_SwayMemMap() As Double Implements IPlugin_Game.Get_SwayMemMap
        Return Sway_MemMap
    End Function

    Public Function Get_SurgeMemMap() As Double Implements IPlugin_Game.Get_SurgeMemMap
        Return Surge_MemMap
    End Function

    Public Function Get_Extra1MemMap() As Double Implements IPlugin_Game.Get_Extra1MemMap
        Return Extra1_MemMap
    End Function

    Public Function Get_Extra2MemMap() As Double Implements IPlugin_Game.Get_Extra2MemMap
        Return Extra2_MemMap
    End Function

    Public Function Get_Extra3MemMap() As Double Implements IPlugin_Game.Get_Extra3MemMap
        Return Extra3_MemMap
    End Function

    Public Function Get_Vibe1_Output() As String Implements IPlugin_Game.Get_Vibe1_Output
        Return Vibe_1_Output
    End Function

    Public Function Get_Vibe2_Output() As String Implements IPlugin_Game.Get_Vibe2_Output
        Return Vibe_2_Output
    End Function

    Public Function Get_Vibe3_Output() As String Implements IPlugin_Game.Get_Vibe3_Output
        Return Vibe_3_Output
    End Function

    Public Function Get_Vibe4_Output() As String Implements IPlugin_Game.Get_Vibe4_Output
        Return Vibe_4_Output
    End Function

    Public Function Get_Vibe5_Output() As String Implements IPlugin_Game.Get_Vibe5_Output
        Return Vibe_5_Output
    End Function

    Public Function Get_Vibe6_Output() As String Implements IPlugin_Game.Get_Vibe6_Output
        Return Vibe_6_Output
    End Function

    Public Function Get_Vibe7_Output() As String Implements IPlugin_Game.Get_Vibe7_Output
        Return Vibe_7_Output
    End Function

    Public Function Get_Vibe8_Output() As String Implements IPlugin_Game.Get_Vibe8_Output
        Return Vibe_8_Output
    End Function

    Public Function Get_Vibe9_Output() As String Implements IPlugin_Game.Get_Vibe9_Output
        Return Vibe_9_Output
    End Function

    Public ReadOnly Property PluginOptions() As String Implements IPlugin_Game.PluginOptions
        Get
            Return _PluginOptions
        End Get
    End Property
#End Region
End Class