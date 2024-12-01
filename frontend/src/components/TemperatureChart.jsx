import React, { useEffect, useState } from "react";
import { Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  TimeScale
} from 'chart.js';
import 'chartjs-adapter-date-fns';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  TimeScale
);

const TemperatureChart = ({ sensorData }) => {
  const [chartData, setChartData] = useState({
    datasets: [{
      label: "Temperature Sensor 1",
      data: [],
      fill: false,
      borderColor: "red",
      tension: 0.1,
    }]
  });

  useEffect(() => {
    if (sensorData && sensorData.length > 0) {
      
      const sensor1Data = sensorData
        .filter(data => data.sensorId === 1)
        .map(data => ({
          x: new Date(data.timestamp),
          y: data.value
        }));

      setChartData({
        datasets: [{
          label: "Temperature Sensor 1",
          data: sensor1Data,
          fill: false,
          borderColor: "red",
          tension: 0.1,
        }]
      });
    }
  }, [sensorData]);

  const options = {
    responsive: true,
    scales: {
      x: {
        type: 'time',
        time: {
          unit: 'second',
          displayFormats: {
            second: 'HH:mm:ss'
          }
        },
        title: {
          display: true,
          text: 'Time'
        }
      },
      y: {
        min: 15,
        max: 35,
        title: {
          display: true,
          text: 'Temperature (°C)'
        }
      }
    },
    plugins: {
      title: {
        display: true,
        text: 'Real-time Temperature Data - Sensor 1'
      }
    },
    animation: {
      duration: 0
    }
  };

  return (
    <div>
      <h2>Temperature Sensor 1 Data</h2>
      <Line data={chartData} options={options} />
    </div>
  );
};

export default TemperatureChart;