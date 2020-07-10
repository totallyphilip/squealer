Public Class Textify

    Public Class AlertClass

        Private _Message As String
        Private _Beep As Boolean

        Public ReadOnly Property Message As String
            Get
                Return _Message
            End Get
        End Property

        Public Property Beep As Boolean
            Get
                Return _Beep
            End Get
            Set(value As Boolean)
                _Beep = value
            End Set
        End Property

        Public Sub New(ByVal m As String, ByVal b As Boolean)
            _Message = m
            _Beep = b
        End Sub

    End Class

    Public Class ColorScheme
        Private _OldForegroundColor As ConsoleColor = Console.ForegroundColor
        Private _OldBackgroundColor As ConsoleColor = Console.BackgroundColor
        Private _NewForegroundColor As ConsoleColor = Console.ForegroundColor
        Private _NewBackgroundColor As ConsoleColor = Console.BackgroundColor
        Public Sub Activate()
            Console.ForegroundColor = _NewForegroundColor
            Console.BackgroundColor = _NewBackgroundColor
        End Sub
        Public Sub Deactivate()
            Console.ForegroundColor = _OldForegroundColor
            Console.BackgroundColor = _OldBackgroundColor
        End Sub
        Public Sub New()
        End Sub
        Public Sub New(fg As ConsoleColor)
            _NewForegroundColor = fg
        End Sub
        Public Sub New(fg As ConsoleColor, bg As ConsoleColor)
            _NewForegroundColor = fg
            _NewBackgroundColor = bg
        End Sub
    End Class

    Public Shared InfoAlert As New AlertClass("!", False)
    Public Shared WarningAlert As New AlertClass("WARNING:", False)
    Public Shared ErrorAlert As New AlertClass("ERROR:", False)

    Public Enum eSeverity
        info
        warning
        [error]
    End Enum

    Public Enum eBullet
        Carat
        Hash
        Arrow
        Star
    End Enum

    ' Write a blank line.
    Public Shared Sub SayNewLine()
        SayNewLine(1)
    End Sub

    ' Write a number of blank lines.
    Public Shared Sub SayNewLine(ByVal lines As Integer)
        For i As Integer = 1 To lines
            Console.WriteLine()
        Next
    End Sub

    Public Shared Function PluralS(ByVal s As String, ByVal count As Integer) As String
        If count = 1 Then
            Return s
        Else
            Return s & "s"
        End If
    End Function

#Region " Bullet "

    Public Shared Sub SayBulletLine(ByVal b As eBullet, ByVal s As String, fg As ConsoleColor)
        SayBullet(b, s, 0, True, New ColorScheme(fg))
    End Sub
    Public Shared Sub SayBulletLine(ByVal b As eBullet, ByVal s As String, cs As ColorScheme)
        SayBullet(b, s, 0, True, cs)
    End Sub
    Public Shared Sub SayBulletLine(ByVal b As eBullet, ByVal s As String)
        SayBullet(b, s, 0, True, New ColorScheme())
    End Sub
    Public Shared Sub SayBullet(ByVal b As eBullet, ByVal s As String)
        SayBullet(b, s, 0, False, New ColorScheme())
    End Sub
    Public Shared Sub SayBullet(ByVal b As eBullet, ByVal s As String, ByVal indent As Integer, cs As ColorScheme)
        SayBullet(b, s, indent, False, cs)
    End Sub
    Public Shared Sub SayBulletLine(ByVal b As eBullet, ByVal s As String, ByVal indent As Integer)
        SayBullet(b, s, indent, True, New ColorScheme())
    End Sub
    Public Shared Sub SayBulletLine(ByVal b As eBullet, ByVal s As String, ByVal indent As Integer, cs As ColorScheme)
        SayBullet(b, s, indent, True, cs)
    End Sub

    Private Shared Sub SayBullet(ByVal b As eBullet, ByVal s As String, ByVal indent As Integer, ByVal newline As Boolean, cs As ColorScheme)
        Dim bullet As String = String.Empty
        Select Case b
            Case eBullet.Arrow
                bullet = "->"
            Case eBullet.Carat
                bullet = ">"
            Case eBullet.Hash
                bullet = "#"
            Case eBullet.Star
                bullet = "*"
        End Select
        Textify.Write(bullet & " " & s, indent, newline, cs)
    End Sub

#End Region

    Public Shared Sub SayCharMaxWidth(ByVal c As Char)
        Console.WriteLine(RepeatedChar(c, Console.WindowWidth - 1))
    End Sub

    Public Shared Function RepeatedChar(ByVal c As Char, ByVal n As Integer) As String
        Return New String(c, n)
    End Function

    Public Shared Sub SayCentered(ByVal s As String)
        SayCentered(s, True)
    End Sub

    Public Shared Sub SayCentered(ByVal s As String, ByVal newline As Boolean)
        ' Specify MidPointRounding option because simple (console.width-string.length)/2 is returning 60 for both 59.5 and 60.5.
        Dim difference As Integer = Console.WindowWidth - s.Length
        If difference < 1 Then
            difference = 0
        End If
        Dim spaces As Integer = CInt(Math.Round((difference) / 2, 0, MidpointRounding.AwayFromZero))
        Console.Write(RepeatedChar(" "c, spaces) & s)
        If newline Then
            Console.WriteLine()
        End If
    End Sub

    Public Shared Sub SayError(ByVal s As String)
        SayError(s, eSeverity.error)
    End Sub

    Public Shared Sub SayError(ByVal s As String, ByVal severity As eSeverity)
        SayError(s, severity, True)
    End Sub

    Public Shared Sub SayError(ByVal s As String, ByVal severity As eSeverity, ByVal newline As Boolean)
        Select Case severity
            Case eSeverity.error
                Textify.Write(ErrorAlert.Message, New ColorScheme(ConsoleColor.White, ConsoleColor.DarkRed))
                If ErrorAlert.Beep Then
                    Console.Beep()
                End If
            Case eSeverity.info
                Textify.Write(InfoAlert.Message, New ColorScheme(ConsoleColor.White, ConsoleColor.DarkBlue))
                If InfoAlert.Beep Then
                    Console.Beep()
                End If
            Case eSeverity.warning
                Textify.Write(WarningAlert.Message, New ColorScheme(ConsoleColor.Black, ConsoleColor.Gray))
                If WarningAlert.Beep Then
                    Console.Beep()
                End If
        End Select
        Textify.Write(" " & s, New ColorScheme(ConsoleColor.Red))
        If newline Then
            Console.WriteLine()
        End If
    End Sub

    Public Shared X As New Random
    Public Shared Function RandomX(ByVal s As String) As Integer
        Return X.Next(Console.WindowWidth - s.Length)
    End Function


#Region " Write "


    Public Shared Sub Write(s As String)
        Write(s, 0, False, New ColorScheme())
    End Sub
    Public Shared Sub Write(s As String, fg As ConsoleColor, Optional lm As eLineMode = eLineMode.Full)
        Write(s, 0, False, New ColorScheme(fg), lm)
    End Sub
    Public Shared Sub Write(s As String, fg As ConsoleColor)
        Write(s, 0, False, New ColorScheme(fg))
    End Sub

    Public Shared Sub Write(s As String, cs As ColorScheme)
        Write(s, 0, False, cs)
    End Sub

    Public Shared Sub Write(s As String, indent As Integer)
        Write(s, indent, False, New ColorScheme())
    End Sub
    Public Shared Sub Write(s As String, indent As Integer, cs As ColorScheme)
        Write(s, indent, False, cs)
    End Sub

    Public Shared Sub WriteLine(s As String)
        Write(s, 0, True, New ColorScheme())
    End Sub
    Public Shared Sub WriteLine(s As String, indent As Integer)
        Write(s, indent, True, New ColorScheme())
    End Sub
    Public Shared Sub WriteLine(s As String, cs As ColorScheme)
        Write(s, 0, True, cs)
    End Sub
    Public Shared Sub WriteLine(s As String, fg As ConsoleColor)
        Write(s, 0, True, New ColorScheme(fg))
    End Sub
    Public Shared Sub WriteLine(s As String, indent As Integer, cs As ColorScheme)
        Write(s, indent, True, cs)
    End Sub

    Public Shared Sub WriteToEol(c As Char)
        WriteToEol(c, New ColorScheme())
    End Sub
    Public Shared Sub WriteToEol(c As Char, cs As ColorScheme)
        cs.Activate()
        Console.Write(New String(c, Console.BufferWidth - Console.CursorLeft - 1))
        cs.Deactivate()
    End Sub

    Public Enum eLineMode
        Full
        Truncate
    End Enum
    Private Shared Sub Write(s As String, indent As Integer, ByVal newline As Boolean, cs As ColorScheme, Optional lm As eLineMode = eLineMode.Full)

        cs.Activate()

        If lm = eLineMode.Truncate Then
            If Console.CursorLeft + s.Length >= Console.BufferWidth Then
                s = s.Substring(0, Console.BufferWidth - Console.CursorLeft - 4).TrimEnd & "..."
            End If
        End If

        Dim words As List(Of String) = s.Split((New Char() {" "c})).ToList

        For i As Integer = 1 To words.Count - 1
            words(i) = " " & words(i)
        Next
        For Each word As String In words
            If Console.BufferWidth > indent Then
                If Console.CursorLeft + word.Length >= Console.BufferWidth Then
                    Console.WriteLine()
                    word = word.Trim
                    If Not Console.CursorLeft + word.Length + indent >= Console.BufferWidth Then
                        Console.Write(New String(" "c, indent))
                    End If
                End If
            End If
            Console.Write(word)
        Next

        If newline Then
            Console.WriteLine()
        End If

        cs.Deactivate()

    End Sub

#End Region


End Class
