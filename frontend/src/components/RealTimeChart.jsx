import { useState, useEffect } from "react";
import { HubConnectionBuilder, HttpTransportType, LogLevel } from "@microsoft/signalr";
import { Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  TimeScale,
} from "chart.js";
import "chartjs-adapter-date-fns";
import Skeleton from "react-loading-skeleton";
import "react-loading-skeleton/dist/skeleton.css";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  TimeScale
);

const RealTimeChart = () => {
  const [data, setData] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7290/sensorHub", {
        withCredentials: true,
        transport: HttpTransportType.WebSockets,
      })
      .configureLogging(LogLevel.Debug)
      .withAutomaticReconnect()
      .build();

    const startConnection = async () => {
      try {
        await connection.start();
        console.log("Connected to SignalR hub");
        connection.on("ReceiveSensorData", (newSensorData) => {
          console.log("New data received:", newSensorData);

          // Update state with new data (keep last 20 entries per sensor)
          setData((prevData) => {
            const updatedData = { ...prevData };
            newSensorData.forEach((dataPoint) => {
              if (!updatedData[dataPoint.sensorId]) {
                updatedData[dataPoint.sensorId] = [];
              }
              updatedData[dataPoint.sensorId] = [...updatedData[dataPoint.sensorId], dataPoint].slice(-20);
            });
            return updatedData;
          });
          setLoading(false);
        });
      } catch (error) {
        console.error("Connection failed: ", error);
        setTimeout(startConnection, 5000);
      }
    };

    startConnection();

    return () => {
      connection.stop();
    };
  }, []);

  const options = {
    responsive: true,
    scales: {
      x: {
        type: "time",
        time: {
          unit: "second",
        },
        title: {
          display: true,
          text: "Timestamp",
        },
      },
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: "Sensor Value",
        },
      },
    },
    plugins: {
      title: {
        display: true,
        text: "Live Sensor Data",
      },
    },
  };

  return (
    <div className="p-4">
      <h2 className="text-2xl font-bold mb-4">Real-time Sensor Data</h2>
      {loading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {Array.from({ length: 5 }).map((_, index) => (
            <div key={index} className="bg-white p-4 rounded-lg shadow-md">
              <Skeleton height={200} />
            </div>
          ))}
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {Object.keys(data).map((sensorId) => (
            <div key={sensorId} className="bg-white p-4 rounded-lg shadow-md">
              <h3 className="text-xl font-semibold mb-2">Sensor {sensorId}</h3>
              <Line
                data={{
                  labels: data[sensorId].map((d) => new Date(d.timestamp)),
                  datasets: [
                    {
                      label: `Sensor ${sensorId}`,
                      data: data[sensorId].map((d) => d.value),
                      fill: false,
                      borderColor: "rgba(75,192,192,1)",
                    },
                  ],
                }}
                options={options}
              />
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default RealTimeChart;