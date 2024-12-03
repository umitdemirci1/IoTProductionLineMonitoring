import { useState } from "react";
import axios from "axios";
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const ReportPage = () => {
  const [productionLineId, setProductionLineId] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [analytics, setAnalytics] = useState(null);
  const [error, setError] = useState("");

  const fetchAnalytics = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7290/api/ProductionLine/production-line/${productionLineId}/analytics`,
        { params: { startDate, endDate } }
      );
      console.log("API Response:", response.data);
      setAnalytics(response.data);
      setError("");
    } catch (error) {
      console.error("Error fetching analytics:", error);
      setError("Error fetching analytics. Please check the console for more details.");
    }
  };

  const options = {
    responsive: true,
    scales: {
      x: {
        title: {
          display: true,
          text: "Sensor",
        },
      },
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: "Average Value",
        },
      },
    },
    plugins: {
      title: {
        display: true,
        text: "Production Line Analytics",
      },
    },
  };

  return (
    <div className="p-4">
      <h2 className="text-2xl font-bold mb-4">Production Line Analytics</h2>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          fetchAnalytics();
        }}
        className="mb-4"
      >
        <label className="block mb-2">
          Production Line ID:
          <input
            type="text"
            value={productionLineId}
            onChange={(e) => setProductionLineId(e.target.value)}
            className="border p-2 rounded w-full"
          />
        </label>
        <label className="block mb-2">
          Start Date:
          <input
            type="datetime-local"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="border p-2 rounded w-full"
          />
        </label>
        <label className="block mb-2">
          End Date:
          <input
            type="datetime-local"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="border p-2 rounded w-full"
          />
        </label>
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
        >
          Get Analytics
        </button>
      </form>

      {error && <p className="text-red-500">{error}</p>}

      {analytics && (
        <div>
          <h3 className="text-xl font-semibold mb-4">Analytics Data</h3>
          <Bar
            data={{
              labels: analytics.map((sensor) => sensor.sensorName),
              datasets: [
                {
                  label: "Average Value",
                  data: analytics.map((sensor) => sensor.average),
                  backgroundColor: "rgba(75,192,192,0.4)",
                  borderColor: "rgba(75,192,192,1)",
                  borderWidth: 1,
                },
              ],
            }}
            options={options}
          />
        </div>
      )}
    </div>
  );
};

export default ReportPage;