Imports System.Data

Class MainWindow
    Private Sub BtnDelZoo_Click(sender As Object, e As RoutedEventArgs) Handles btnDelZoo.Click
        If Not IsNothing(listZoos.SelectedItem) Then
            Dim zooId As Integer = CType(listZoos.SelectedItem, DataRowView)("Id")
            Dim location As String = CType(listZoos.SelectedItem, DataRowView)("Location")
            Dim zooDA As New MyZooManagerDataSetTableAdapters.ZooTableAdapter()
            Try
                zooDA.Delete(zooId, location)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowZoos()
                ShowZooAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnAddZoo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddZoo.Click
        If textHelper.Text.Length > 0 Then
            Try
                Dim zooDA As New MyZooManagerDataSetTableAdapters.ZooTableAdapter()
                zooDA.Insert(textHelper.Text)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowZoos()
                ShowZooAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnAddAnimal_Click(sender As Object, e As RoutedEventArgs) Handles btnAddAnimal.Click
        If textHelper.Text.Length > 0 Then
            Try
                Dim animalDA As New MyZooManagerDataSetTableAdapters.AnimalTableAdapter()
                animalDA.Insert(textHelper.Text)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowAllAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnDelZooAnimal_Click(sender As Object, e As RoutedEventArgs) Handles btnDelZooAnimal.Click
        If Not IsNothing(listZooAnimals.SelectedItem) Then
            Try
                Dim zooId As Integer = listZoos.SelectedValue
                Dim animalId As Integer = listZooAnimals.SelectedValue
                Dim zooAnimalDA As New MyZooManagerDataSetTableAdapters.ZooAnimalTableAdapter()
                zooAnimalDA.RemoveAnimalFromZoo(zooId, animalId)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowZooAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnAddAnimalToZoo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddAnimalToZoo.Click
        If Not IsNothing(listZoos.SelectedItem) And Not IsNothing(listAllAnimals.SelectedItem) Then
            Try
                Dim zooId As Integer = listZoos.SelectedValue
                Dim animalId As Integer = listAllAnimals.SelectedValue
                Dim zooAnimalDA As New MyZooManagerDataSetTableAdapters.ZooAnimalTableAdapter()
                zooAnimalDA.Insert(zooId, animalId)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowZooAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnUpdateZoo_Click(sender As Object, e As RoutedEventArgs) Handles btnUpdateZoo.Click
        If Not IsNothing(listZoos.SelectedItem) Then
            Try
                Dim zooId As Integer = listZoos.SelectedValue
                Dim zooDA As New MyZooManagerDataSetTableAdapters.ZooTableAdapter()
                zooDA.UpdateZoo(textHelper.Text, zooId)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowZoos()
                ShowZooAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnUpdateAnimal_Click(sender As Object, e As RoutedEventArgs) Handles btnUpdateAnimal.Click
        If Not IsNothing(listAllAnimals.SelectedItem) Then
            Try
                Dim animalId As Integer = listAllAnimals.SelectedValue
                Dim animalDA As New MyZooManagerDataSetTableAdapters.AnimalTableAdapter()
                animalDA.UpdateAnimal(textHelper.Text, animalId)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowAllAnimals()
            End Try
        End If
    End Sub

    Private Sub BtnDelAnimal_Click(sender As Object, e As RoutedEventArgs) Handles btnDelAnimal.Click
        If Not IsNothing(listAllAnimals.SelectedItem) Then
            Dim zooId As Integer = CType(listZoos.SelectedItem, DataRowView)("Id")
            Dim location As String = CType(listZoos.SelectedItem, DataRowView)("Location")
            Dim zooDA As New MyZooManagerDataSetTableAdapters.ZooTableAdapter()
            Try
                zooDA.Delete(zooId, location)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                ShowAllAnimals()
            End Try
        End If
    End Sub

    Private Sub ListZoos_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles listZoos.SelectionChanged
        ShowZooAnimals()
        If Not IsNothing(listZoos.SelectedItem) Then
            textHelper.Text = CType(listZoos.SelectedItem, DataRowView)("Location")
        Else
            textHelper.Text = ""
        End If
    End Sub

    Private Sub ListZooAnimals_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles listZooAnimals.SelectionChanged
        If Not IsNothing(listZooAnimals.SelectedItem) Then
            textHelper.Text = CType(listZooAnimals.SelectedItem, DataRowView)("Name")
        Else
            textHelper.Text = ""
        End If
    End Sub

    Private Sub ListAllAnimals_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles listAllAnimals.SelectionChanged
        If Not IsNothing(listAllAnimals.SelectedItem) Then
            textHelper.Text = CType(listAllAnimals.SelectedItem, DataRowView)("Name")
        Else
            textHelper.Text = ""
        End If
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ShowZoos()
        ShowAllAnimals()
    End Sub

    Private Sub ShowZoos()
        Dim zoosDA As New MyZooManagerDataSetTableAdapters.ZooTableAdapter()
        Dim zooDT As MyZooManagerDataSet.ZooDataTable = zoosDA.GetData()
        listZoos.SelectedValuePath = "Id"
        listZoos.DisplayMemberPath = "Location"
        listZoos.ItemsSource = zooDT.DefaultView
    End Sub

    Private Sub ShowAllAnimals()
        Dim animalsDA As New MyZooManagerDataSetTableAdapters.AnimalTableAdapter()
        Dim animalsDT As MyZooManagerDataSet.AnimalDataTable = animalsDA.GetData()
        listAllAnimals.DisplayMemberPath = "Name"
        listAllAnimals.SelectedValuePath = "Id"
        listAllAnimals.ItemsSource = animalsDT.DefaultView
    End Sub

    Private Sub ShowZooAnimals()
        If Not IsNothing(listZoos.SelectedItem) Then
            Dim item As DataRowView = listZoos.SelectedItem
            Dim ZooId As Integer = item("Id")
            Dim zooAnimalDA As New MyZooManagerDataSetTableAdapters.AnimalTableAdapter()
            Dim zooAnimalDT As DataTable = zooAnimalDA.GetDataByZoo(ZooId)
            listZooAnimals.DisplayMemberPath = "Name"
            listZooAnimals.SelectedValuePath = "Id"
            listZooAnimals.ItemsSource = zooAnimalDT.DefaultView
        Else
            listZooAnimals.ItemsSource = Nothing
        End If
    End Sub
End Class
