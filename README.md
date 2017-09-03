# CQRSAzure

## CQRS on Windows Azure

Despite numerous articles and the favourable experiences of enterprise application developers, CQRS has not caught on as a mainstream development architecture (in the way that, for example, MVC has).

I believe that two things are needed in order to demonstrate the architecture and increase its adoption: (1) examples to demonstrate the advantages of scalability, simplicity, testing and read/write independence and (2) tooling to allow developers rapidly to assemble the CQRS application in a graphical model.

![Designer](https://github.com/MerrionComputing/CQRSAzure/blob/master/CQRSAzure/Images/Aggregate_Projection_Events_QueryDefinition_CommandDefinition.PNG)

**Project Description**
A project to create an IDE template, plug ins, documentation and related material to facilitate the rapid creation of CQRS based solutions on Azure

This project is intended to provide a quick start IDE template and plug in and various documentation, techniques and so on to allow the rapid creation of CQRS based projects sitting on Windows Azure.

It will facilitate the code generation of  Aggregates (aggregate roots) ,  Events ,  Projections ,  Query Definitions and  Command Definitions which will build the underlying model of a CQRS / ES based application. This will mean using  Tools and Designers so that the model can be assembled visually inside the IDE.

## Prerequisites

In order to compile the source code for this plug in you will need to have Visual Studio 2017 and to have installed the Visual Studio SDK.

## Beginners guide

I recommend reading the WIKI pages for this project - and there is a "Recommended Reading" page there too to help you get started with the concepts of CQRS and of event sourcing, or if you have 50 minutes to spare take a look at this YouTube presentation: [https://youtu.be/kpM5gCLF1Zc]
