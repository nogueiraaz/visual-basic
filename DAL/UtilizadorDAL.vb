Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
' Classe responsável pelas operações de base de dados para Utilizadores
Public Class UtilizadorDAL
    
    ' Inserir novo utilizador na base de dados
    Public Shared Function Inserir(utilizador As Utilizador) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                ' Query SQL com parâmetros
                Dim query As String = "INSERT INTO utilizadores (nome, turma, contacto) VALUES (@nome, @turma, @contacto)"
                Using cmd As New MySqlCommand(query, conn)
                    ' Adicionar parâmetros para evitar SQL injection
                    cmd.Parameters.AddWithValue("@nome", utilizador.Nome)
                    cmd.Parameters.AddWithValue("@turma", utilizador.Turma)
                    cmd.Parameters.AddWithValue("@contacto", utilizador.Contacto)
                    
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao inserir utilizador: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Listar todos os utilizadores da base de dados
    Public Shared Function Listar() As List(Of Utilizador)
        Dim utilizadores As New List(Of Utilizador)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return utilizadores
                
                Dim query As String = "SELECT * FROM utilizadores"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim util As New Utilizador()
                            util.Id = reader.GetInt32("id")
                            util.Nome = reader.GetString("nome")
                            util.Turma = reader.GetString("turma")
                            util.Contacto = reader.GetString("contacto")
                            utilizadores.Add(util)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao listar utilizadores: " & ex.Message)
        End Try
        Return utilizadores
    End Function
    
    ' Atualizar dados de um utilizador existente
    Public Shared Function Atualizar(utilizador As Utilizador) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "UPDATE utilizadores SET nome=@nome, turma=@turma, contacto=@contacto WHERE id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", utilizador.Id)
                    cmd.Parameters.AddWithValue("@nome", utilizador.Nome)
                    cmd.Parameters.AddWithValue("@turma", utilizador.Turma)
                    cmd.Parameters.AddWithValue("@contacto", utilizador.Contacto)
                    
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao atualizar utilizador: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Eliminar um utilizador da base de dados
    Public Shared Function Eliminar(id As Integer) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "DELETE FROM utilizadores WHERE id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao eliminar utilizador: " & ex.Message)
            Return False
        End Try
    End Function
End Class