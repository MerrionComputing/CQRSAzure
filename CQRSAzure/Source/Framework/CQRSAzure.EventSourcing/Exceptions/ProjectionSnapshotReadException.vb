﻿Imports System.Security.Permissions
Imports System.Runtime.Serialization
Imports System

''' <summary>
''' An exception has occured reading from a projection snapshot
''' </summary>
<Serializable()>
Public Class ProjectionSnapshotReadException
    Inherits ProjectionSnapshotExceptionBase

    Public Sub New(AggregateNameIn As String,
                   AggregateKeyIn As String,
                   ProjectionNameIn As String,
                   Optional MessageIn As String = "",
                   Optional innerExceptionIn As Exception = Nothing)
        MyBase.New(AggregateNameIn, AggregateKeyIn, ProjectionNameIn, MessageIn, innerExceptionIn)
    End Sub

End Class
