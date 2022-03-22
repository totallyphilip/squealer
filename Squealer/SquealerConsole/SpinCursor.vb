Public Class SpinCursor

    Const TicksPerSecond = 10000000 ' 10 million

    Private _Index As Integer = 0
    Private _Symbols As New List(Of Char)
    Private _AnimationsPerSecond As Integer
    Private _LastTick As Long = DateTime.Now.Ticks

    Public Sub New()
        Me.New("/—\|", 4)
    End Sub

    Public Sub New(s As String)
        Me.New(s, 4)
    End Sub

    Public Sub New(AnimationsPerSecond As Integer)
        Me.New("/—\|", AnimationsPerSecond)
    End Sub

    Public Sub New(s As String, AnimationsPerSecond As Integer)
        _Symbols.AddRange(s.ToCharArray)
        _AnimationsPerSecond = AnimationsPerSecond
        Console.Write(CurrentSymbol)
    End Sub

    Private Function CurrentSymbol() As String

        If Console.CursorLeft > Console.BufferWidth - 2 Then
            Return "."
        Else
            Return _Symbols(_Index)
        End If

    End Function

    Public Sub DoStep(final As Boolean)

        Console.Write(Chr(8) & ".")

        If DateTime.Now.Ticks - _LastTick > TicksPerSecond / _AnimationsPerSecond Then
            _LastTick = DateTime.Now.Ticks
            _Index += 1
            If _Index >= _Symbols.Count Then
                _Index = 0
            End If
        End If

        If final Then
            Console.WriteLine("*")
        Else
            Console.Write(CurrentSymbol)
        End If

    End Sub

    Public Sub Animate()
        DoStep(False)
    End Sub

    Public Sub Finish()
        DoStep(True)
    End Sub

End Class