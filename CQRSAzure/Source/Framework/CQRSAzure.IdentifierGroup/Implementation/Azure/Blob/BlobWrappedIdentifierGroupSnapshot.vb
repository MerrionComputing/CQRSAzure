Namespace Azure.Blob

    ''' <summary>
    ''' A wrapper for a snapshot of the aggregate identifiers in a given group as at a given date/time
    ''' </summary>
    Public NotInheritable Class BlobWrappedIdentifierGroupSnapshot(Of TAggregateKey)
        Inherits BlobWrappedIdentifierGroupSnapshot

        Private ReadOnly m_members As IEnumerable(Of TAggregateKey)
        ''' <summary>
        ''' The individual keys that are members of this identifier group
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Members As IEnumerable(Of TAggregateKey)
            Get
                If (m_members IsNot Nothing) Then
                    Return m_members
                Else
                    Return New List(Of TAggregateKey)()
                End If
            End Get
        End Property

        Public Sub New(asOfdateIn As Date, ByVal membersIn As IEnumerable(Of TAggregateKey))
            MyBase.New(asOfdateIn)
            If membersIn IsNot Nothing Then
                m_members = membersIn
            End If
        End Sub

    End Class

    ''' <summary>
    ''' A wrapper for a snapshot of the aggregate identifiers in a given group as at a given date/time
    ''' </summary>
    Public MustInherit Class BlobWrappedIdentifierGroupSnapshot

        Private ReadOnly m_AsOfDate As DateTime
        ''' <summary>
        ''' The effective date/time for which this collection of identifiers are in the group
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AsOfDate As DateTime
            Get
                Return m_AsOfDate
            End Get
        End Property

        Protected Sub New(ByVal asOfdateIn As DateTime)
            m_AsOfDate = asOfdateIn
        End Sub

    End Class

End Namespace