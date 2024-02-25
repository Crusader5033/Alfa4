# Peer Communication Program Documentation

## Overview

This documentation provides an overview of a peer communication program developed in C#. The program allows peers to communicate with each other over TCP and UDP protocols. It enables discovery of other peers in the network, sending and receiving messages, and maintaining a message history.
![Bez názvu](https://github.com/Crusader5033/Alfa4/assets/113086006/1a0b4b1d-cce0-4586-96f9-0d282564770b)

## Program Components

The program consists of several classes:

1. **PeerSettings**: Represents the configuration settings for a peer.
2. **UdpListener**: Listens for UDP messages from other peers.
3. **TcpConnection**: Manages a TCP connection with a remote host.
4. **Peer**: Orchestrates the peer communication process, including discovery, message exchange, and history management.
5. **Program**: The entry point of the program, responsible for initializing and starting the peer.

## Class Documentation

### PeerSettings

- **Properties**:
  - `PeerId`: Gets or sets the unique identifier of the peer.
  - `DiscoveryPort`: Gets or sets the port used for peer discovery.
  - `DiscoveryIntervalSeconds`: Gets or sets the interval (in seconds) for peer discovery.
- **Methods**:
  - `LoadFromFile(filePath)`: Loads peer settings from a specified XML file.

### UdpListener

- **Events**:
  - `MessageReceived`: Raised when a message is received from other peers.
- **Methods**:
  - `StartListening()`: Starts listening for incoming UDP messages.
  - `Close()`: Closes the UDP listener.

### TcpConnection

- **Properties**:
  - `Port`: Gets or sets the port number of the TCP connection.
- **Methods**:
  - `Connect()`: Connects to the remote host.
  - `SendMessage(message)`: Sends a message to the remote host.
  - `Disconnect()`: Disconnects from the remote host.
  - `ReceiveMessages()`: Receives messages from the remote host.

### Peer

- **Constructor**:
  - Initializes a new instance of the `Peer` class.
- **Methods**:
  - `StartListeningForMessages()`: Starts listening for incoming TCP messages.
  - `ListenForMessages()`: Listens for incoming TCP messages.
  - `StartListening()`: Starts listening for UDP messages.
  - `StartDiscovery()`: Starts the peer discovery process.
  - `DiscoverPeers()`: Discovers other peers in the network.
  - `SendDiscoveryRequest()`: Sends a discovery request to other peers.
  - `HandleDiscoveryResponse(message, remoteEP)`: Handles a response received during discovery.
  - `HandleHandshake(peerId, remoteEP)`: Handles the handshake process with a peer.
  - `PrepareHistoryResponse()`: Prepares a response containing message history.
  - `ParseJsonMessage(message)`: Parses a JSON message into a dictionary.
  - `Stop()`: Stops the peer activities and connections.

### Program

- **Main Method**:
  - Initializes and starts the peer communication process.

## Usage

To use the program:
1. Initialize a `Peer` instance with appropriate settings.
2. Start the peer by calling `StartDiscovery()` and `StartListening()` methods.
3. Communicate with other peers using TCP and UDP protocols.
4. Handle incoming messages and manage message history.

## Configuration
You can configura the connection in App.config located in Alfa4/bin/Debug/net6.0
Here is deafult config:

##
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<peerSettings>
		<peerId>PlantationOwner</peerId>
		<discoveryPort>9876</discoveryPort>
		<discoveryIntervalSeconds>5</discoveryIntervalSeconds>
	</peerSettings>
</configuration>

   
## Issues
Only UDP and TCP connection works. Theres some problems with inconsistency so its ideal to run program more than once. 

## Testing
![Výstřižek](https://github.com/Crusader5033/Alfa4/assets/113086006/5471000e-fd8c-49db-9982-02a98ba16df6)

Succesfull UDP discovery and succesfull TCP connection,sometimes i even get response with message history.
![Výstřižekě](https://github.com/Crusader5033/Alfa4/assets/113086006/d241f1f2-272b-499a-960e-533618c8329e)

UDP onlyt(not useable now)
![Samotne UDP](https://github.com/Crusader5033/Alfa4/assets/113086006/b1b8a674-d86d-4777-911e-19e90dfa6242)



## Conclusion

The peer communication program provides a framework for building peer-to-peer applications. It allows seamless communication between peers over TCP and UDP protocols, facilitating message exchange and discovery in a networked environment.
