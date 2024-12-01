// App.jsx
import { useEffect, useState } from "react";
import { startSignalRConnection, subscribeToSensorData } from "./services/signalR";
import TemperatureChart from "./components/TemperatureChart";

const MAX_HISTORY = 20;

function App() {
  const [sensorData, setSensorData] = useState([]);

  useEffect(() => {
    let connection;

    const initializeSignalR = async () => {
      connection = await startSignalRConnection();

      subscribeToSensorData((data) => {
        setSensorData((prevData) => {
          // Filter for temperature sensor 1 only
          if (data.sensorId !== 1) return prevData;
          console.log("Received sensor data here:", data);
          const newData = [...prevData, data];
          // Keep only last N readings
          return newData.slice(-MAX_HISTORY);
        });
      });
    };

    initializeSignalR();

    return () => {
      if (connection) {
        connection.stop();
      }
    };
  }, []);

  return (
    <div>
      <h1>Real-Time Sensor Data</h1>
      <TemperatureChart sensorData={sensorData} />
    </div>
  );
}

export default App;