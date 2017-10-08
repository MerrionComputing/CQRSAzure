
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands
    Public Class CommandEventContext
        Implements ICommandEventContext
        Implements ISerializable

        Private ReadOnly m_source As String
        Public ReadOnly Property Source As String Implements ICommandEventContext.Source
            Get
                Return m_source
            End Get
        End Property

        Private ReadOnly m_userName As String
        Public ReadOnly Property Username As String Implements ICommandEventContext.Username
            Get
                Return m_userName
            End Get
        End Property

        Private ReadOnly m_token As String
        Public ReadOnly Property Token As String Implements ICommandEventContext.Token
            Get
                Return m_token
            End Get
        End Property

        Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(Source), Source)
                info.AddValue(NameOf(Username), Username)
                info.AddValue(NameOf(Token), Token)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            If Not (info Is Nothing) Then
                m_source = info.GetString(NameOf(Source))
                m_userName = info.GetString(NameOf(Username))
                m_token = info.GetString(NameOf(Token))
            End If

        End Sub

        Protected Sub New(Optional ByVal contextIn As CommandEventContext = Nothing)
            If (contextIn IsNot Nothing) Then
                m_source = contextIn.Source
                m_userName = contextIn.Username
                m_token = contextIn.Token
            End If
        End Sub

        Private Sub New(ByVal sourceIn As String,
                        ByVal usernameIn As String,
                        ByVal tokenIn As String)

            m_source = sourceIn
            m_userName = usernameIn
            m_token = tokenIn

        End Sub

        Public Shared Function Create(ByVal sourceIn As String,
                        ByVal usernameIn As String,
                                      ByVal tokenIn As String) As CommandEventContext

            Return New CommandEventContext(sourceIn, usernameIn, tokenIn)

        End Function

    End Class
End Namespace