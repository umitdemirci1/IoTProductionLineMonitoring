import { useState } from "react";
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
import zoomPlugin from "chartjs-plugin-zoom";
import axios from "axios";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  TimeScale,
  zoomPlugin
);

const HistoricalDataChart = () => {
  const [sensorId, setSensorId] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [chartData, setChartData] = useState(null);

  const fetchHistoricalData = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.get("https://localhost:7290/api/SensorData/SensorData", {
        params: {
          sensorId: sensorId,
          startDate: startDate,
          endDate: endDate,
        },
      });

      const data = response.data;

      // Downsample data to show at most 1000 data points
      const sampleRate = Math.ceil(data.length / 1000);
      const sampledData = data.filter((_, index) => index % sampleRate === 0);

      const formattedData = {
        labels: sampledData.map((d) => new Date(d.timestamp)),
        datasets: [
          {
            label: `Sensor ${sensorId}`,
            data: sampledData.map((d) => d.value),
            backgroundColor: "rgba(75,192,192,0.4)",
            borderColor: "rgba(75,192,192,1)",
            borderWidth: 1,
          },
        ],
      };

      setChartData(formattedData);
    } catch (error) {
      console.error("Error fetching historical data:", error);
    }
  };

  const options = {
    responsive: true,
    scales: {
      x: {
        type: "time",
        time: {
          unit: "day",
        },
        title: {
          display: true,
          text: "Date",
        },
        ticks: {
          autoSkip: true,
          maxTicksLimit: 10,
          callback: function(value, index, values) {
            return new Date(value).toLocaleDateString();
          },
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
        text: "Historical Sensor Data",
      },
      zoom: {
        pan: {
          enabled: true,
          mode: "xy",
        },
        zoom: {
          wheel: {
            enabled: true,
          },
          pinch: {
            enabled: true,
          },
          mode: "xy",
        },
      },
    },
  };

  return (
    <div className="p-4">
      <h2 className="text-2xl font-bold mb-4">Historical Sensor Data</h2>
      <form onSubmit={fetchHistoricalData} className="mb-4">
        <div className="mb-2">
          <label className="block font-semibold">Sensor ID:</label>
          <input
            type="text"
            value={sensorId}
            onChange={(e) => setSensorId(e.target.value)}
            className="border border-gray-300 p-2 rounded-md w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block font-semibold">Start Date:</label>
          <input
            type="datetime-local"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="border border-gray-300 p-2 rounded-md w-full"
            required
          />
        </div>
        <div className="mb-2">
          <label className="block font-semibold">End Date:</label>
          <input
            type="datetime-local"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="border border-gray-300 p-2 rounded-md w-full"
            required
          />
        </div>
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
        >
          Get Data
        </button>
      </form>

      {chartData && <Line data={chartData} options={options} />}
    </div>
  );
};

export default HistoricalDataChart;