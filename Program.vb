Imports System.Windows.Forms

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        
        Try
            Dim loginForm As New FormLogin()
            
            If loginForm.ShowDialog() = DialogResult.OK Then
                Application.Run(New FormPrincipal(loginForm.UtilizadorLogado, loginForm.TipoUtilizador))
            End If
        Catch ex As Exception
            MessageBox.Show("Erro: " & ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub
End Module