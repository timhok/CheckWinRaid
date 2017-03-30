
Imports System.Text.RegularExpressions

Module Module1
    Dim AlertSet As Boolean = False
    Dim AlertUrl As String = ""
    Dim AlertState As Boolean = False

    Dim rawinfo As String

    Sub Main(ByVal Args() As String)
        Dim sArg As String = ""
        If Args.Count > 0 Then sArg = Args(0)

        Select Case sArg
            Case "-h"
                Console.WriteLine(String.Format("{0} {1}.{2}", My.Application.Info.AssemblyName, My.Application.Info.Version.Major, My.Application.Info.Version.Minor))
                Console.WriteLine(String.Format("Copyright (C) {0} by {1}, {2}", "2017-03", "Timhok", "http://github.com/"))
                Console.WriteLine()
                Console.WriteLine(String.Format("Usage: {0} [option]", "checkwinraid.exe"))
                Console.WriteLine()
                Console.WriteLine("Options:")
                Console.WriteLine(String.Format("{0}{1} {2}{0}{3}", vbTab, "-h", vbTab, "Display this help"))
                Console.WriteLine(String.Format("{0}{1} {2}{0}{3}", vbTab, "-c", vbTab, "Alert url. If any volume change its status - request will be made at defined url"))
            Case "-c"
                AlertSet = True
                If Args.Count < 2 Then
                    Console.WriteLine("No Alert url present - ignoring")
                Else
                    AlertUrl = Args(1)
                End If


            Case Else
        End Select

        Dim p As Process
        p = New Process()
        p.StartInfo.FileName = Environ("windir") & "\system32\diskpart.exe"
        p.StartInfo.UseShellExecute = False
        p.StartInfo.Arguments = "/s list_volume"
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.CreateNoWindow = True
        p.Start()
        p.WaitForExit()
        rawinfo = p.StandardOutput.ReadToEnd()
        Dim volumes As MatchCollection = Regex.Matches(rawinfo, " {2,}(.*?) {2,}([A-Z]{1}) {2,}.* {2,}NTFS {2,}(.*?) {2,}(.*?) {2}(.*) {0,2}")
        ' 1 - number
        ' 2 - letter
        ' 3 - type
        ' 4 - size
        ' 5 - status
        For Each volume As Match In volumes
            Dim VolumeLetter As String = volume.Groups(2).Value
            Dim VolumeStatus As String = volume.Groups(5).Value.Split(" ")(0)
            Dim VolumeHealthy As Boolean = (VolumeStatus.Contains("Исправен") Or VolumeStatus.Contains("Healty"))
            Console.WriteLine(String.Format("{0} {1}", VolumeLetter, VolumeHealthy))

            If Not VolumeHealthy Then
                AlertState = True
            End If
        Next

        If AlertSet And AlertState Then
            Dim webClient As New System.Net.WebClient
            On Error Resume Next
            webClient.DownloadString(AlertUrl)
        End If

    End Sub


End Module
