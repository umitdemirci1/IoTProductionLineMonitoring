import {
  HubConnectionBuilder,
  LogLevel,
  HttpTransportType,
} from "@microsoft/signalr";

let connection;

export const startSignalRConnection = async () => {
  connection = new HubConnectionBuilder()
    .withUrl("https://localhost:7290/sensorHub", {
      withCredentials: true,
      transport: HttpTransportType.WebSockets,
    })
    .configureLogging(LogLevel.Debug)
    .withAutomaticReconnect()
    .build();

  try {
    console.log("SignalR connection state:", connection.state);
    await connection.start();
    console.log("SignalR connected successfully!");
    console.log("Connection ID:", connection.connectionId);
    console.log("Transport type:", connection.transport?.name);

    connection.onreconnecting((error) => {
      console.log("SignalR attempting to reconnect...", error);
    });

    connection.onreconnected((connectionId) => {
      console.log("SignalR reconnected with ID:", connectionId);
    });

    connection.onclose((error) => {
      console.log("SignalR connection closed", error);
    });
  } catch (error) {
    console.error("SignalR connection failed:", error);
    if (error.innerError) {
      console.error("Inner error details:", error.innerError);
    }
    throw error;
  }

  return connection;
};

export const subscribeToSensorData = (callback) => {
  if (connection) {
    connection.on("ReceiveSensorData", (data) => {
      console.log("Sensor data received:", data);
      callback(data);
    });
  }
};
export const stopSignalRConnection = async () => {
  if (connection) {
    try {
      await connection.stop();
      console.log("SignalR disconnected!");
    } catch (error) {
      console.error("Error stopping SignalR connection:", error);
    }
  }
};
