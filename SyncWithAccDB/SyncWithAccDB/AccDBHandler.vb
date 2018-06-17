﻿Imports System.Data.OleDb

Public Class AccDBHandler

    Dim Source As String = ""
    Dim FileName As String = ""
    Dim CompletePath As String = ""
    Dim ConnectionOpen As Boolean = False
    Public DataSetCollection As New DataSet

    Public Sub SetSource(SourceStr As String, FileNameStr As String)
        Source = SourceStr
        FileName = FileNameStr
        CompletePath = Source & FileName
    End Sub

    Public Sub CreateNewDB(Optional Overwrite As Boolean = False)

        If CompletePath = "" Then
            MsgBox("No Source set")
            Exit Sub
        End If

        If Overwrite = True Then

            If IO.File.Exists(CompletePath) Then
                IO.File.Delete(CompletePath)
            End If

        Else

            If IO.File.Exists(CompletePath) Then
                MsgBox("DB already exists")
                Exit Sub
            End If

        End If


        If Not IO.Directory.Exists(Source) Then
            IO.Directory.CreateDirectory(Source)
        End If

        Dim ADOXCatalog As New ADOX.Catalog
        Dim ADOXTable As New ADOX.Table
        Dim ADOXTable2 As New ADOX.Table
        Dim ADOXIndex As New ADOX.Index

        ADOXCatalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & CompletePath)

        On Error Resume Next

        ADOXCatalog.ActiveConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & CompletePath

        ADOXTable.Name = "Routes"
        ADOXTable.Columns.Append("RouteName", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("RouteDistance", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("DemandEco", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("DemandBusiness", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("DemandFirst", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("OfferEco", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("OfferBusiness", ADOX.DataTypeEnum.adVarWChar)
        ADOXTable.Columns.Append("OfferFirst", ADOX.DataTypeEnum.adVarWChar)

        ADOXCatalog.Tables.Append(ADOXTable)
        ADOXTable.Indexes.Append(ADOXIndex)

        ADOXTable = Nothing
        ADOXTable2 = Nothing
        ADOXCatalog = Nothing
        ADOXIndex = Nothing

    End Sub

    Public Sub ReadDatabase(Table As String, ReadAll As Boolean, Optional SearchString As String = "")
        Dim Query As String
        Dim MDBConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & CompletePath
        Dim DatabaseConnection As New OleDbConnection

        If ConnectionOpen = False Then
            DatabaseConnection.ConnectionString = MDBConnectionString
            Try
                DatabaseConnection.Open()
            Catch ex As Exception
                MsgBox(ex)
                Exit Sub
            End Try
        End If

        Select Case ReadAll
            Case True

                Query = "SELECT * FROM " & Table

                Dim DatabaseCommand As New OleDbCommand(Query, DatabaseConnection)
                Dim DatabaseAdapter As New OleDbDataAdapter(DatabaseCommand)

                DatabaseAdapter.Fill(DataSetCollection)

            Case False



        End Select

    End Sub

    Public Sub WriteToDatabase(ArrayToWrite As String())

        For Each item In ArrayToWrite
            MsgBox(item)
        Next

    End Sub

End Class
