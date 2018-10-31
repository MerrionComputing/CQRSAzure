Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup
''' <summary>
''' Base class to be implemented by any class that supplies an identity group for an aggregate identifier
''' </summary>
Public MustInherit Class IdentityGroupBase(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Implements IIdentifierGroup(Of TAggregateIdentifier, TAggregateKey)

    Public Const GROUPNAME_INSTANCE As String = "Instance"
    Public Const GROUPNAME_ALL As String = "All"


    Private ReadOnly m_classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)
    Private ReadOnly m_parentGroup As IIdentifierGroup(Of TAggregateIdentifier, TAggregateKey)

    ' Members can be pre-loaded from a parent group and then classified in parrallel
    Private ReadOnly m_members As New System.Collections.Concurrent.ConcurrentDictionary(Of TAggregateKey, IClassifierDataSourceHandler.EvaluationResult)

    ''' <summary>
    ''' The class implementing the identity group must provide its name
    ''' </summary>
    Public MustOverride ReadOnly Property Name As String Implements IIdentifierGroup.Name

    ''' <summary>
    ''' The name of the outer parent group of which all members must be members of to be checked for membership
    ''' of this group
    ''' </summary>
    Public ReadOnly Property ParentGroupName As String Implements IIdentifierGroup.ParentGroupName
        Get
            If (m_parentGroup IsNot Nothing) Then
                Return m_parentGroup.Name
            Else
                Return GROUPNAME_ALL
            End If
        End Get
    End Property

    ''' <summary>
    ''' The classifier to run to (re)generate this identity group
    ''' </summary>
    ''' <remarks>
    ''' This may be null for "Identity" or "All" identity groups as these do not need a 
    ''' projection to decide if any entity is in or out of the group
    ''' </remarks>
    Public Overridable ReadOnly Property Classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)
        Get
            Return m_classifier
        End Get
    End Property



    ''' <summary>
    ''' Get the list of aggregate identifiers in the group as at the given date (or as of now if no date is passed in)
    ''' </summary>
    ''' <param name="AsOfDate">
    ''' The effective date for which we want to know thwe group membership.
    ''' (If this is not passed in then as of current date and time is assumed)
    ''' </param>
    ''' <returns>
    ''' A collection of aggregate identifiers considered to be "inside" the group
    ''' </returns>
    Public Overridable Function GetMembers(Optional ByVal AsOfDate As DateTime = Nothing) As IEnumerable(Of TAggregateIdentifier)


        If (m_parentGroup IsNot Nothing) Then
            'TODO : Get the members of the parent group and preload the members dictionary.. 
        Else
            'TODO : Get the members of the ALL group and preload the members dictionary.. 
        End If

        'TODO : Process the members

        'Return those memebers marked as in the group
        Return m_members.Where(AddressOf IsInGroup)


    End Function

    Public Shared Function IsInGroup(ByVal pair As KeyValuePair(Of TAggregateKey, IClassifierDataSourceHandler.EvaluationResult)) As Boolean
        Return (pair.Value = IClassifierDataSourceHandler.EvaluationResult.Include)
    End Function

    Public Sub New(Optional ByVal classifierToUse As IClassifier(Of TAggregateIdentifier, TAggregateKey) = Nothing,
                   Optional ByVal parentGroup As IIdentifierGroup(Of TAggregateIdentifier, TAggregateKey) = Nothing)
        If classifierToUse IsNot Nothing Then
            m_classifier = classifierToUse
        End If
        If parentGroup IsNot Nothing Then
            m_parentGroup = parentGroup
        End If
    End Sub

End Class
