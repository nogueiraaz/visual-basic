Imports MySql.Data.MySqlClient
Imports System.Windows.Forms

' Classe responsável pela conexão com a base de dados MySQL
Public Class DatabaseConnection
    ' String de conexão para XAMPP (localhost, root, sem password)
    Private Shared connectionString As String = "Server=localhost;Database=livraria;Uid=root;Pwd=;"
    
    ' Método para obter uma conexão aberta
    Public Shared Function GetConnection() As MySqlConnection
        Try
            Dim conn As New MySqlConnection(connectionString)
            conn.Open()
            Return conn
        Catch ex As Exception
            MessageBox.Show("Erro ao conectar à base de dados: " & ex.Message)
            Return Nothing
        End Try
    End Function
    
    ' Método para testar se a conexão funciona
    Public Shared Function TestarConexao() As Boolean
        Try
            Dim conn = GetConnection()
            If conn IsNot Nothing Then
                conn.Close()
                Return True
            End If
            Return False
        Catch ex As Exception
            MessageBox.Show("Erro na conexão: " & ex.Message)
            Return False
        End Try
    End Function
End Class