<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Program
End Class

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        
        ' Mostrar formulário de login
        Dim loginForm As New FormLogin()
        
        If loginForm.ShowDialog() = DialogResult.OK Then
            ' Login bem-sucedido, abrir formulário principal
            Application.Run(New FormPrincipal(loginForm.UtilizadorLogado, loginForm.TipoUtilizador))
        End If
    End Sub
End Module