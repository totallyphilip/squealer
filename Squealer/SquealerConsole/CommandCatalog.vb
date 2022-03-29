Namespace CommandCatalog

    Public Enum eCommandCategory
        folder
        file
        other
    End Enum

    Public Class CommandSwitchOptionList

        Private _Items As New List(Of CommandSwitchOption)
        Public ReadOnly Property Items As List(Of CommandSwitchOption)
            Get
                Return _Items
            End Get
        End Property

        'Public Function Find(k As String) As SwitchOption
        '    Return _Items.Find(Function(x) x.Keyword = k.ToLower)
        'End Function

        Public Sub New()
            ' empty list
        End Sub

        Public Sub New(s As String)

            ' option1|option2|option3|etc.
            For Each o As String In s.Split((New Char() {"|"c}))
                _Items.Add(New CommandSwitchOption(o))
            Next

        End Sub

    End Class

    Public Class CommandSwitchOption

        Private _Keyword As String
        Private _Help As String = String.Empty

        Public ReadOnly Property Keyword As String
            Get
                Return _Keyword
            End Get
        End Property
        Public ReadOnly Property Help As String
            Get
                Return _Help
            End Get
        End Property

        Public Sub New(s As String)

            s = s.ToLower
            _Keyword = s.Split((New Char() {";"c}))(0)
            If s.Contains(";") Then
                _Help = s.Split((New Char() {";"c}))(1)
            End If

        End Sub

    End Class

    Public Class CommandSwitchList

        Private _Items As New List(Of CommandSwitch)
        Public ReadOnly Property Items As List(Of CommandSwitch)
            Get
                Return _Items
            End Get
        End Property

    End Class

    Public Class CommandSwitch

        Private _Options As New CommandSwitchOptionList
        Public ReadOnly Property Options As CommandSwitchOptionList
            Get
                Return _Options
            End Get
        End Property

        Private _Keyword As String
        Public ReadOnly Property Keyword As String
            Get
                Return _Keyword
            End Get
        End Property

        Private _Help As String = String.Empty
        Public ReadOnly Property Help As String
            Get
                Return _Help
            End Get
        End Property

        Private _IsDefault As Boolean = False
        Public ReadOnly Property IsDefault As Boolean
            Get
                Return _IsDefault
            End Get
        End Property

        Public Sub New(s As String, isdefault As Boolean)
            Me.New(s)
            _IsDefault = isdefault
        End Sub

        Public Sub New(s As String)

            ' Format: keyword;help[:mutually;help|exclusive;help|options;help]
            ' Example: x;exact
            ' Example: t;type:p;stored procedure|fn;scalar function|if;inline table-valued function|tf;multi-statement table-valued function|v;view

            Dim key As String = s.Split((New Char() {":"c}))(0)

            ' keyword;help
            Me._Keyword = key.Split((New Char() {";"c}))(0)
            If key.Contains(";") Then
                Me._Help = key.Split((New Char() {";"c}))(1)
            End If

            If s.Contains(":") Then
                _Options = New CommandSwitchOptionList(s.Split((New Char() {":"c}))(1))
            End If

        End Sub

    End Class

    Public Class CommandDefinitionList

        Private _Items As New List(Of CommandDefinition)
        Public ReadOnly Property Items As List(Of CommandDefinition)
            Get
                Return _Items
            End Get
        End Property

        Public Function FindCommand(s As String) As CommandCatalog.CommandDefinition
            s = s.Trim.ToLower
            Return _Items.Find(Function(x) x.Keyword = s OrElse x.Shortkeyword = s)
        End Function

        Public Sub ShowHelpCatalog(showhidden As Boolean)

            Dim maxcommandlength As Integer = 0
            For Each c As CommandDefinition In _Items
                If c.ShortAndLongKeyword.Length > maxcommandlength Then
                    maxcommandlength = c.ShortAndLongKeyword.Length
                End If
            Next

            Dim categories As String()
            categories = System.Enum.GetNames(GetType(eCommandCategory))
            For Each cat As String In categories

                Textify.WriteLine("// " & cat & " //", ConsoleColor.White)
                Console.WriteLine()

                For Each c As CommandDefinition In Items.Where(Function(x) x.CommandCategory.ToString = cat AndAlso (x.Visible Or showhidden))
                    If c.Visible Then
                        Textify.SayBullet(Textify.eBullet.Arrow, "")
                    Else
                        Textify.SayBullet(Textify.eBullet.Arrow, "", 0, New Textify.ColorScheme(ConsoleColor.White, ConsoleColor.DarkRed))
                    End If
                    Textify.Write(c.ShortAndLongKeyword.ToUpper, ConsoleColor.Green)
                    Textify.WriteLine(New String(" "c, maxcommandlength - c.ShortAndLongKeyword.Length) & " " & c.CommandShortHelp, maxcommandlength + 4)
                Next

                Console.WriteLine()

            Next

            Textify.WriteLine("// function keys //", ConsoleColor.White)
            Console.WriteLine()
            Textify.SayBullet(Textify.eBullet.Arrow, "")
            Textify.Write("F7" & New String(" "c, maxcommandlength - 2), ConsoleColor.Green)
            Textify.WriteLine(" Your command history.", maxcommandlength + 4)
            Textify.SayBullet(Textify.eBullet.Arrow, "")
            Textify.Write("F11" & New String(" "c, maxcommandlength - 3), ConsoleColor.Green)
            Textify.WriteLine(" Full-screen mode on/off.", maxcommandlength + 4)
            'Textify.SayBulletLine(Textify.eBullet.Arrow, "F7" & New String(" "c, maxcommandlength - 2) & " Your command history.", maxcommandlength + 4)
            'Textify.SayBulletLine(Textify.eBullet.Arrow, "F11" & New String(" "c, maxcommandlength - 3) & " Full-screen mode on/off.", maxcommandlength + 4)
            Console.WriteLine()

        End Sub

    End Class

    Public Class CommandDefinition

        Public Const WildcardText As String = "<wildcard>"
        Public Const FilenameText As String = "<filename>"

        Private _Options As New CommandSwitchList
        Public ReadOnly Property Options As CommandSwitchList
            Get
                Return _Options
            End Get
        End Property

        Private _IgnoreSwitches As Boolean = False
        Public Property IgnoreSwitches As Boolean
            Get
                Return _IgnoreSwitches
            End Get
            Set(value As Boolean)
                _IgnoreSwitches = value
            End Set
        End Property

        Private _Keyword As String
        Public ReadOnly Property Keyword As String
            Get
                Return _Keyword.ToLower
            End Get
        End Property

        Private _Shortkeyword As String = String.Empty
        Public ReadOnly Property Shortkeyword As String
            Get
                Return _Shortkeyword.ToLower
            End Get
        End Property

        Public ReadOnly Property ShortAndLongKeyword As String
            Get
                Dim s As String = _Keyword
                If Not String.IsNullOrWhiteSpace(_Shortkeyword) Then
                    s = s & "|" & _Shortkeyword
                End If
                Return s
            End Get
        End Property

        Public ReadOnly Property ShortestKeyword As String
            Get
                If String.IsNullOrEmpty(_Shortkeyword) Then
                    Return _Keyword
                Else
                    Return _Shortkeyword
                End If
            End Get
        End Property

        Private _Summary As String
        Public ReadOnly Property Summary As String
            Get
                Return _Summary
            End Get
        End Property

        Private _Details As String = String.Empty
        Public ReadOnly Property Details As String
            Get
                Return _Details.Replace("{keyword}", Me.Keyword).Replace("{options}", Me.ParameterDefinition).Replace("{command}", Me.Keyword.ToUpper)
            End Get
        End Property

        Public ReadOnly Property CommandShortHelp As String
            Get
                Return _Summary.Replace("{keyword}", Me.Keyword).Replace("{options}", Me.ParameterDefinition).Replace("{command}", Me.Keyword.ToUpper)
            End Get
        End Property

        Private _ParameterDefinition As String = String.Empty
        Public ReadOnly Property ParameterDefinition As String
            Get
                Dim s As String = String.Empty
                If Not String.IsNullOrWhiteSpace(_ParameterDefinition) Then
                    s = _ParameterDefinition
                    If Not _ParameterRequired Then
                        s = String.Format("[{0}]", s)
                    End If
                End If
                Return s
            End Get
        End Property

        Private _ParameterRequired As Boolean
        Public ReadOnly Property ParameterRequired As Boolean
            Get
                Return _ParameterRequired
            End Get
        End Property

        Private _CommandCategory As eCommandCategory
        Public ReadOnly Property CommandCategory As eCommandCategory
            Get
                Return _CommandCategory
            End Get
        End Property

        Private _Examples As New List(Of String)
        Public ReadOnly Property Examples As List(Of String)
            Get
                Return _Examples
            End Get
        End Property

        Private _CanFileSearch As Boolean = False
        Public ReadOnly Property CanFileSearch As Boolean
            Get
                Return _CanFileSearch
            End Get
        End Property

        Private _Visible As Boolean = True
        Public Property Visible As Boolean
            Get
                Return _Visible
            End Get
            Set(value As Boolean)
                _Visible = value
            End Set
        End Property

        Public Sub New(k() As String, help() As String, cat As eCommandCategory, req As Boolean, cansearch As Boolean)

            Me.New(k, help, cat)

            If cansearch Then
                _CanFileSearch = True
                _ParameterDefinition = String.Format("{0}|#", CommandCatalog.CommandDefinition.WildcardText)
                _ParameterRequired = req
                'todo: uh, no, this makes too many NEW calls, which means the config settings are repeatedly loaded. there has to be a better way.
                Dim temp As New Settings() ' need an instance to expose a property name
                For Each s As String In New SquealerObjectTypeCollection().ObjectTypesOptionString(True).Split((New Char() {"|"c}))
                    _Options.Items.Add(New CommandCatalog.CommandSwitch(s))
                Next
                _Options.Items.Add(New CommandSwitch(String.Format("x;exact filename match, override {0} setting", Constants.WildcardAsterisks)))
                _Options.Items.Add(New CommandSwitch("today;files with today's date"))
                _Options.Items.Add(New CommandSwitch("cs;case-sensitive text search"))
                _Options.Items.Add(New CommandSwitch("code;with pre/post code"))
                _Options.Items.Add(New CommandSwitch("u;uncommitted, case-sensitive per github.com/microsoft/vscode/issues/10633"))
                '_Options.Items.Add(New CommandSwitch("fi;include flags, case-sensitive:<flags>;pipe-delimited flags")) ' this won't work until command parser is more robust
                '_Options.Items.Add(New CommandSwitch("fx;exclude flags, case-sensitive:<flags>;pipe-delimited flags"))
            End If

        End Sub

        Public Sub New(k() As String, help() As String, cat As eCommandCategory, p As String, req As Boolean)

            Me.New(k, help, cat)
            _ParameterDefinition = p
            _ParameterRequired = req

        End Sub

        Public Sub New(k() As String, help() As String, cat As eCommandCategory)

            ' Example command definitions:

            ' generate|gen;Generate stuff.|Extended help goes here.

            ' generate|gen -x;exact filename match -o:alt;alter (do not drop)|t;test script -w;with:e;encryption -t;type:p;stored procedure|fn;scalar function|if;inline table-valued function|tf;multi-statement table-valued function|v;view -cs;case-sensitive text search
            ' fix -x;exact filename match -o:alt;alter (do not drop)|t;test script -w;with:e;encryption -t;type:p;stored procedure|fn;scalar function|if;inline table-valued function|tf;multi-statement table-valued function|v;view -c;convert to:p;stored procedure|fn;scalar function|if;inline table-valued function|tf;multi-statement table-valued function|v;view -cs;case-sensitive text search

            _Keyword = k(0)
            If k.Length > 1 Then
                _Shortkeyword = k(1)
            End If

            _Summary = help(0)

            If help.Length > 1 Then
                _Details = help(1)
            End If

            _CommandCategory = cat

        End Sub

        Public Sub ShowHelp()

            Textify.Write(Me.Keyword.ToUpper, ConsoleColor.Green)
            If Me.Keyword <> Me.ShortestKeyword Then
                Textify.Write(String.Format(" (or {0})", Me.Shortkeyword.ToUpper))
            End If

            Console.WriteLine()
            Textify.SayCharMaxWidth("-"c)

            Textify.WriteLine(Me.CommandShortHelp & " " & Me.Details, 2)
            Textify.SayCharMaxWidth("-"c)
            Console.WriteLine()

            Console.Write("Usage: ")
            Textify.Write(Me.Keyword, ConsoleColor.Green)

            For Each opt1 As CommandCatalog.CommandSwitch In Me.Options.Items
                Textify.Write(String.Format(" -{0}", opt1.Keyword), ConsoleColor.White)

                If opt1.Options.Items.Count > 0 Then
                    Textify.Write(":")
                    Dim separator As String = String.Empty

                    For Each o As CommandCatalog.CommandSwitchOption In opt1.Options.Items
                        Textify.Write(separator & o.Keyword, ConsoleColor.Cyan)
                        separator = "|"
                    Next

                End If
            Next

            If Not String.IsNullOrEmpty(Me.ParameterDefinition) Then
                Textify.Write(" " & Me.ParameterDefinition, ConsoleColor.Green)
            End If
            If Me.CanFileSearch Then
                Textify.Write(" [/<searchtext>]", ConsoleColor.Green)
            End If

            Console.WriteLine()

            Textify.SayNewLine()

            If Me.Options.Items.Count > 0 Then
                Console.WriteLine("Switches:")
                For Each s As CommandCatalog.CommandSwitch In Me.Options.Items
                    Textify.Write(String.Format("  -{0}", s.Keyword), ConsoleColor.White)
                    If s.Options.Items.Count > 0 Then
                        Textify.Write(":", ConsoleColor.White)
                    End If
                    Textify.Write(String.Format(" ({0})", s.Help), ConsoleColor.DarkGray)
                    If s.IsDefault Then
                        Textify.WriteLine(" DEFAULT")
                    Else
                        Textify.SayNewLine()
                    End If
                    If s.Options.Items.Count > 0 Then
                        Dim separator As String = String.Empty
                        For Each o As CommandCatalog.CommandSwitchOption In s.Options.Items
                            Textify.Write(String.Format("      {0} ", o.Keyword), ConsoleColor.Cyan)
                            Textify.WriteLine(String.Format("({0})", o.Help), ConsoleColor.DarkGray)
                        Next
                    End If
                Next
                Console.WriteLine()

            End If

            If Me.CanFileSearch Then
                Dim temp As New Settings() ' need an instance to expose a property name
                Textify.WriteLine(String.Format("See {0} setting. I.E. 'foo bar' may be treated as '*foo*bar*'", Constants.WildcardAsterisks))
                Console.WriteLine()
            End If

            Console.WriteLine("Example:")

            If Not Me.ParameterRequired Then
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Carat, String.Format("{0}", Me.ShortestKeyword))
            End If

            For Each example As String In Me.Examples
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Carat, example.Replace("%", Me.ShortestKeyword))
            Next

            If Me.CanFileSearch Then
                'Console.WriteLine()
                'Textify.SayBullet(Textify.eBullet.Carat, String.Format("{0} -fx:i|J -- exclude objects flagged with ""i"" and ""J""", Me.ShortestKeyword))
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Carat, String.Format("{0} dbo.*|*abc*|dbo.xyz -- pipe-delimited {1}s", Me.ShortestKeyword, WildcardText))
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Carat, String.Format("{0} # -- open the file dialog", Me.ShortestKeyword))
            End If

            Console.WriteLine()

        End Sub

    End Class

End Namespace
