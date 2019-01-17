Imports System
Imports System.Reflection
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table.Untyped

    ''' <summary>
    ''' An implementation of event streams using Azure tables for non type-specified streams
    ''' </summary>
    ''' <remarks>
    ''' Care must be taken to make sure this remains compatible with the type-safe implementation
    ''' </remarks>
    Public MustInherit Class TableEventStreamBaseUntyped
        Inherits TableEventStreamBase
        Implements IEventStreamWriterUntyped

        Public Overloads ReadOnly Property DomainName As String Implements IEventStreamUntypedIdentity.DomainName
            Get
                Return MyBase.DomainName
            End Get
        End Property


        Public ReadOnly Property AggregateTypeName As String Implements IEventStreamUntypedIdentity.AggregateTypeName
            Get
                Return MyBase.AggregateClassName
            End Get
        End Property

        Private ReadOnly m_InstanceKey As String
        Public ReadOnly Property InstanceKey As String Implements IEventStreamUntypedIdentity.InstanceKey
            Get
                Return m_InstanceKey
            End Get
        End Property


        Public ReadOnly Property RecordCount As ULong Implements IEventStreamUntyped.RecordCount
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStreamUntyped.LastAddition
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Async Function AppendEvent(EventInstance As IEvent,
                               Optional ExpectedTopSequence As Long = 0,
                               Optional Version As UInteger = 1) As Task Implements IEventStreamWriterUntyped.AppendEvent

            Throw New NotImplementedException()

        End Function

        ''' <summary>
        ''' Does an event stream already exist for this Domain/Type/Instance
        ''' </summary>
        ''' <remarks>
        ''' This can be used for e.g. checking it exists as part of a validation
        ''' </remarks>
        Public Function Exists() As Task(Of Boolean) Implements IEventStreamUntyped.Exists

            If (MyBase.Table IsNot Nothing) Then
                Return MyBase.Table.ExistsAsync()
            Else
                Return Task(Of Boolean).FromResult(Of Boolean)(False)
            End If


        End Function

        ''' <summary>
        ''' Create the underlying stream storage if it does not already exist
        ''' </summary>
        Public Async Function CreateIfNotExists() As Task Implements IEventStreamUntyped.CreateIfNotExists

            Dim alreadyExists As Boolean = Await Me.Exists()
            If (Not alreadyExists) Then
                ' Nothing to do currently - the table is always created
                ' This may, however, change in future
            End If

        End Function

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriterUntyped.SetContext
            m_context = writerContext
        End Sub

        Protected Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                          Optional writeAccess As Boolean = False,
                          Optional connectionStringName As String = "",
                          Optional settings As ITableSettings = Nothing)

            MyBase.New(identifier.DomainName,
                       identifier.AggregateTypeName,
                       writeAccess,
                       connectionStringName,
                       settings)

            m_InstanceKey = identifier.InstanceKey



        End Sub


    End Class
End Namespace