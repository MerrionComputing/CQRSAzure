Namespace Local.File
    Public Interface ILocalFileSettings
        Inherits IEventStreamSettings

        ''' <summary>
        ''' The top-level folder where the event streams are stored
        ''' </summary>
        ''' <remarks>
        ''' The folder structure is {root}\{domain}\{aggregate name} with each event stream being stored in an individual 
        ''' file in that folder
        ''' </remarks>
        Property EventStreamRootFolder As String

        ''' <summary>
        ''' The top-level folder where snapshots are stored
        ''' </summary>
        ''' <remarks>
        ''' The folder structure is {root}\{domain}\{projection name} with each snapshot being stored in an individual 
        ''' file in that folder with the file suffix indicating the as-of sequence number
        ''' </remarks>
        Property SnapshotsRootFolder As String


        ''' <summary>
        ''' The different types of serialiser we can use to persist to file
        ''' </summary>
        Enum SerialiserType
            ''' <summary>
            ''' Use this for max performance if only this app will read the files
            ''' </summary>
            Binary = 0
            ''' <summary>
            ''' Use this for readability and file sharing scenarios
            ''' </summary>
            NameValuePairs = 1
        End Enum

        ''' <summary>
        ''' The type of serialiser we use to persist to a local file
        ''' </summary>
        Property UnderlyingSerialiser As SerialiserType


    End Interface
End Namespace