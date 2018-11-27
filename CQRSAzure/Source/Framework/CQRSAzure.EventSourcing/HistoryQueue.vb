
Imports System.Collections.Generic
Imports System.Linq
''' <summary>
''' A queue based data structure that can have a fixed size that automatically rolls off any elements that 
''' exceed that queue size
''' </summary>
''' <typeparam name="TElement">
''' The type-safe element type of this generic queue
''' </typeparam>
''' <remarks>
''' This is used for "Top n elements" type processing of projections
''' </remarks>
Public Class HistoryQueue(Of TElement)
    Inherits Queue(Of TElement)


    Private ReadOnly m_QueueSize As Integer
    Public ReadOnly Property MaximumQueueSize As Integer
        Get
            If (m_QueueSize < 0) Then
#Region "Tracing"
                EventSourcing.LogError("History Queue has an invalid size ( less than zero)")
#End Region
                Return 0
            Else
                Return m_QueueSize
            End If


        End Get
    End Property


    Public Overloads Sub Enqueue(ByVal element As TElement)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Enqueueing an element - " & element.ToString())
#End Region

        MyBase.Enqueue(element)

        If (m_QueueSize > 0) Then
            Dim itemsToRemove As Integer = MyBase.LongCount() - m_QueueSize

            If (itemsToRemove > 0) Then
#Region "Tracing"
                EventSourcing.LogVerboseInfo(itemsToRemove.ToString() & " items to remove from the history queue")
#End Region
                While itemsToRemove > 0
                    MyBase.Dequeue()
                    itemsToRemove = itemsToRemove - 1
                End While
            End If
        End If
    End Sub

    ''' <summary>
    ''' Initialise the queue with a maximum queue size
    ''' </summary>
    ''' <param name="MaxQueueSize">
    ''' The maximum size to allow this queue to grow to
    ''' </param>
    Public Sub New(ByVal MaxQueueSize As Integer)

#Region "Tracing"
        EventSourcing.LogInfo("New history queue created - size " & MaxQueueSize.ToString())
#End Region

        m_QueueSize = MaxQueueSize
    End Sub

End Class
