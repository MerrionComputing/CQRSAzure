Option Explicit On

Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class QueryEventStream_UnitTest


    <TestMethod()>
    Public Sub QueryAggregate_Constructor_TestMethod()

        Dim testObj As New Queries.QueryAggregate(Guid.NewGuid())
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub QueryCompletedEvent_Constructor_TestMethod()

        Dim testObj As Queries.QueryCompletedEvent = Queries.QueryCompletedEvent.Create(DateTime.Now,
                                                                                        123,
                                                                                        "Test complete")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub QueryCompletedEvent_RoundTrip_RecordCount_Serialisation_TestMethod()

        Dim expected As Integer = 321
        Dim actual As Integer = 999

        Dim testObj As Queries.QueryCompletedEvent = Queries.QueryCompletedEvent.Create(DateTime.Now,
                                                                                        expected,
                                                                                        "Test complete")

        Dim cmdObjDeserialised As Queries.QueryCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ResultRecordCount

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub QueryCompletedEvent_RoundTrip_SuccessMessage_Serialisation_TestMethod()

        Dim expected As String = "Unit test success"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCompletedEvent = Queries.QueryCompletedEvent.Create(DateTime.Now,
                                                                                        123,
                                                                                        expected)

        Dim cmdObjDeserialised As Queries.QueryCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.SuccessMessage

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_QueryName_Serialisation_TestMethod()

        Dim expected As String = "Unit test query name"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(Guid.NewGuid(),
                                                                                    expected,
                                                                                     DateTime.Now,
                                                                                    "Unit test source",
                                                                                    "User name",
                                                                                    "Identity Group",
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.QueryName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_QueryId_Serialisation_TestMethod()

        Dim expected As Guid = Guid.NewGuid()
        Dim actual As Guid = Guid.Empty

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    expected,
                                                                                    "Unit testing query",
                                                                                     DateTime.Now,
                                                                                    "Unit test source",
                                                                                    "User name",
                                                                                    "Identity Group",
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.QueryUniqueIdentifier

        Assert.AreEqual(expected, actual)
    End Sub



    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_Source_Serialisation_TestMethod()

        Dim expected As String = "Unit test query source"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                     DateTime.Now,
                                                                                    expected,
                                                                                    "User name",
                                                                                    "Identity Group",
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Source

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_Username_Serialisation_TestMethod()

        Dim expected As String = "Unit test username"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                     DateTime.Now,
                                                                                    "Source",
                                                                                    expected,
                                                                                    "Identity Group",
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Username

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_IdentityGroup_Serialisation_TestMethod()

        Dim expected As String = "Unit test identity group"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                     DateTime.Now,
                                                                                    "Source",
                                                                                    "Test",
                                                                                    expected,
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.IdentityGroupName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryCreatedEvent_RoundTrip_Parameters_Serialisation_TestMethod()

        Dim expected As String = "Unit test query parameters"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                     DateTime.Now,
                                                                                    "Source",
                                                                                    "Test",
                                                                                    "Identity group",
                                                                                    expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.QueryParameters

        Assert.AreEqual(expected, actual)
    End Sub

    Public Sub QueryCreatedEvent_RoundTrip_Date_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2016, 12, 19, 3, 23, 1)
        Dim actual As DateTime = New DateTime(2026, 4, 3)

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(
                                                                                    Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                     expected,
                                                                                    "Source",
                                                                                    "Username",
                                                                                    "Identity Group",
                                                                                    "Parameters"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CreationDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryFatalErrorOccuredEvent_RoundTrip_Date_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2016, 12, 19, 3, 23, 1)
        Dim actual As DateTime = New DateTime(2026, 4, 3)

        Dim testObj As Queries.QueryFatalErrorOccuredEvent = Queries.QueryFatalErrorOccuredEvent.Create(expected,
                                                                                    "Unit testing error message"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryFatalErrorOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ErrorDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryFatalErrorOccuredEvent_RoundTrip_ErrorMessage_Serialisation_TestMethod()

        Dim expected As String = "Unit test error message"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryFatalErrorOccuredEvent = Queries.QueryFatalErrorOccuredEvent.Create(DateTime.Now,
                                                                                                        expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryFatalErrorOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ErrorMessage

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupMemberReturnedEvent_String_RoundTrip_GroupName_Serialisation_TestMethod()

        Dim expected As String = "Unit test identity group"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryIdentityGroupMemberReturnedEvent(Of String) = Queries.QueryIdentityGroupMemberReturnedEvent(Of String).Create(DateTime.Now,
                                                                                                                                                  expected,
                                                                                                                                                  "Unique id"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupMemberReturnedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.IdentityGroupName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupMemberReturnedEvent_String_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As String = "OLJ565M"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryIdentityGroupMemberReturnedEvent(Of String) = Queries.QueryIdentityGroupMemberReturnedEvent(Of String).Create(DateTime.Now,
                                                                                                                                                  "All Cars",
                                                                                                                                                  expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupMemberReturnedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupMemberReturnedEvent_Integer_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As Integer = 123
        Dim actual As Integer = 99

        Dim testObj As Queries.QueryIdentityGroupMemberReturnedEvent(Of Integer) = Queries.QueryIdentityGroupMemberReturnedEvent(Of Integer).Create(DateTime.Now,
                                                                                                                                                  "All Cars",
                                                                                                                                                  expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupMemberReturnedEvent(Of Integer)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupMemberReturnedEvent_GUID_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As Guid = Guid.NewGuid()
        Dim actual As Guid = Guid.Empty

        Dim testObj As Queries.QueryIdentityGroupMemberReturnedEvent(Of Guid) = Queries.QueryIdentityGroupMemberReturnedEvent(Of Guid).Create(DateTime.Now,
                                                                                                                                                  "All Cars",
                                                                                                                                                  expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupMemberReturnedEvent(Of Guid)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        Assert.AreEqual(expected, actual)
    End Sub

    'QueryIdentityGroupRequestedEvent
    <TestMethod()>
    Public Sub QueryIdentityGroupRequestedEvent_RoundTrip_GroupName_Serialisation_TestMethod()

        Dim expected As String = "Unit test identity group"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryIdentityGroupRequestedEvent = Queries.QueryIdentityGroupRequestedEvent.Create(DateTime.Now,
                                                                                                        expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.IdentityGroupName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupRequestedEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(1977, 12, 19, 12, 0, 1)
        Dim actual As DateTime = New DateTime(2012, 2, 2)

        Dim testObj As Queries.QueryIdentityGroupRequestedEvent = Queries.QueryIdentityGroupRequestedEvent.Create(expected,
                                                                                                        "Identity group"
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupReturnedEvent_RoundTrip_GroupName_Serialisation_TestMethod()

        Dim expected As String = "Unit test identity group"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryIdentityGroupReturnedEvent = Queries.QueryIdentityGroupReturnedEvent.Create(DateTime.Now,
                                                                                                        expected
                                                                                     )

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.IdentityGroupName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryIdentityGroupReturnedEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 2, 1, 19, 3, 22)
        Dim actual As DateTime = New DateTime(1998, 12, 31)

        Dim testObj As Queries.QueryIdentityGroupReturnedEvent = Queries.QueryIdentityGroupReturnedEvent.Create(
                                                                                                        expected,
                                                                                                        "Unit test group")

        Dim cmdObjDeserialised As Queries.QueryIdentityGroupReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryProjectionRequestedEvent_String_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 2, 1, 19, 3, 22)
        Dim actual As DateTime = New DateTime(1998, 12, 31)

        Dim testObj As Queries.QueryProjectionRequestedEvent(Of String) = Queries.QueryProjectionRequestedEvent(Of String).Create(
                                                                                                        expected,
                                                                                                        "Unit test group",
                                                                                                        "Unit test aggregate")

        Dim cmdObjDeserialised As Queries.QueryProjectionRequestedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryProjectionRequestedEvent_Integer_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 2, 1, 19, 3, 22)
        Dim actual As DateTime = New DateTime(1998, 12, 31)

        Dim testObj As Queries.QueryProjectionRequestedEvent(Of Int32) = Queries.QueryProjectionRequestedEvent(Of Int32).Create(
                                                                                                        expected,
                                                                                                        "Unit test group",
                                                                                                        1234)

        Dim cmdObjDeserialised As Queries.QueryProjectionRequestedEvent(Of Int32)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryProjectionRequestedEvent_String_RoundTrip_ProjectionName_Serialisation_TestMethod()

        Dim expected As String = "Unit test projection"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryProjectionRequestedEvent(Of String) = Queries.QueryProjectionRequestedEvent(Of String).Create(
                                                                                                        DateTime.UtcNow,
                                                                                                        expected,
                                                                                                        "Unit test aggregate")

        Dim cmdObjDeserialised As Queries.QueryProjectionRequestedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ProjectionName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryProjectionReturnedEvent_String_RoundTrip_ResultsLocation_Serialisation_TestMethod()

        Dim expected As String = "Unit test projection"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryProjectionReturnedEvent(Of String) = Queries.QueryProjectionReturnedEvent(Of String).Create(
                                                                                                       expected,
                                                                                                        DateTime.UtcNow,
                                                                                                        "Unit test aggregate",
                                                                                                        "Results location")

        Dim cmdObjDeserialised As Queries.QueryProjectionReturnedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ProjectionName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryProjectionReturnedEvent_String_RoundTrip_ProjectionName_Serialisation_TestMethod()

        Dim expected As String = "Unit test location"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryProjectionReturnedEvent(Of String) = Queries.QueryProjectionReturnedEvent(Of String).Create(
                                                                                                       "Unit test projection",
                                                                                                        DateTime.UtcNow,
                                                                                                        "Unit test aggregate",
                                                                                                        expected)

        Dim cmdObjDeserialised As Queries.QueryProjectionReturnedEvent(Of String)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ResultsLocation

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryStartedEvent_RoundTrip_ProcessorName_Serialisation_TestMethod()

        Dim expected As String = "Unit test processor"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryStartedEvent = Queries.QueryStartedEvent.Create(DateTime.UtcNow,
                                                                                    expected)

        Dim cmdObjDeserialised As Queries.QueryStartedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Processor

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryStartedEvent_RoundTrip_StartDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 1, 13, 9, 11, 12)
        Dim actual As DateTime = New DateTime(2012, 1, 2)

        Dim testObj As Queries.QueryStartedEvent = Queries.QueryStartedEvent.Create(expected,
                                                                                    "Test processor")

        Dim cmdObjDeserialised As Queries.QueryStartedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ProcessingStartDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    'QueryTransientFaultOccuredEvent
    <TestMethod()>
    Public Sub QueryTransientFaultOccuredEvent_RoundTrip_FaultDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 1, 13, 9, 11, 12)
        Dim actual As DateTime = New DateTime(2012, 1, 2)

        Dim testObj As Queries.QueryTransientFaultOccuredEvent = Queries.QueryTransientFaultOccuredEvent.Create(expected,
                                                                                    "Test transient fault")

        Dim cmdObjDeserialised As Queries.QueryTransientFaultOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.FaultDate.GetValueOrDefault()

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod()>
    Public Sub QueryTransientFaultOccuredEvent_RoundTrip_FaultMessage_Serialisation_TestMethod()

        Dim expected As String = "fault reason"
        Dim actual As String = "Not set"

        Dim testObj As Queries.QueryTransientFaultOccuredEvent = Queries.QueryTransientFaultOccuredEvent.Create(DateTime.UtcNow,
            expected)

        Dim cmdObjDeserialised As Queries.QueryTransientFaultOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.FaultMessage

        Assert.AreEqual(expected, actual)
    End Sub


    <TestMethod()>
    Public Sub QueryCreatedEvent_Constructor_TestMethod()

        Dim testObj As Queries.QueryCreatedEvent = Queries.QueryCreatedEvent.Create(Guid.NewGuid(),
                                                                                    "Unit test query",
                                                                                    Nothing,
                                                                                    "Unit test framework",
                                                                                    "Duncan",
                                                                                    String.Empty,
                                                                                    "")
        Assert.IsNotNull(testObj)

    End Sub




End Class