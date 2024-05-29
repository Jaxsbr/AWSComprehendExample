# AWSComprehendExample

I've always been interested in AI systems similar to "Jarvis", the AI in the movie Iron Man, where Tony Stark talks to the AI with simple language. The AI then performs various difficult tasks that would have otherwise have been done by a human.
e.g. Hey Jarvis, start a new project and call it Iron Suit.

In this repository I demonstrate how to use AWS Comprehend to extract the neccesary information needed from a sentence to determine what type of `dotnet` project to create.

NOTE: This repository serves only as example of integrating and using AWS Comprehend. The different project permutation have not all been tested against the python script.

## Prerequisites

- AWS account
- awscli profile configuration

## Run the console application

Provide a prompt e.g. (Create a new wpf project and name it SuperApp)
AWS Comprehend will extract syntax from the prompt.
A python script will then be invoked with the extracted and matched attributes to create a specified dotnet project.

## Possible matches

### Verbs

- create (Indicates a new projects/solution is to be generate)
- add (Indicates a project needs to be added to an existing solution)
- remove (Indicates a project is to be removed from an existing solutions)

### DotNet project types

- console
- webapi
- wpf
- sln
