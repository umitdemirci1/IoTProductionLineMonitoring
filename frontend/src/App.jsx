import { useState } from "react";
import RealTimeChart from "./components/RealTimeChart";
import HistoricalDataChart from "./components/HistoricalDataChart";

function App() {
  const [activeTab, setActiveTab] = useState("real-time");

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">
        IoT Production Line Monitoring
      </h1>
      <div className="mb-4">
        <button
          className={`px-4 py-2 mr-2 ${activeTab === "real-time" ? "bg-blue-500 text-white" : "bg-gray-200"}`}
          onClick={() => setActiveTab("real-time")}
        >
          Real-time Data
        </button>
        <button
          className={`px-4 py-2 ${activeTab === "historical" ? "bg-blue-500 text-white" : "bg-gray-200"}`}
          onClick={() => setActiveTab("historical")}
        >
          Historical Data
        </button>
      </div>
      <div>
        {activeTab === "real-time" && <RealTimeChart />}
        {activeTab === "historical" && <HistoricalDataChart />}
      </div>
    </div>
  );
}

export default App;
