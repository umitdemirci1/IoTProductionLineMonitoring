// import { useState } from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import RealTimeChart from "./components/RealTimeChart";
import HistoricalDataChart from "./components/HistoricalDataChart";
import ReportPage from "./components/ReportPage";

function App() {
  return (
    <Router>
      <div className="container mx-auto p-4">
        <h1 className="text-3xl font-bold mb-6">IoT Production Line Monitoring</h1>
        <nav className="mb-4">
          <Link to="/" className="px-4 py-2 mr-2 bg-blue-500 text-white rounded">Home</Link>
          <Link to="/report" className="px-4 py-2 bg-blue-500 text-white rounded">Report</Link>
        </nav>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/real-time" element={<RealTimeChart />} />
          <Route path="/historical" element={<HistoricalDataChart />} />
          <Route path="/report" element={<ReportPage />} />
        </Routes>
      </div>
    </Router>
  );
}

function Home() {
  return (
    <div>
      <div className="mb-4">
        <Link to="/real-time" className="px-4 py-2 mr-2 bg-gray-200 rounded">Real-time Data</Link>
        <Link to="/historical" className="px-4 py-2 bg-gray-200 rounded">Historical Data</Link>
      </div>
    </div>
  );
}

export default App;
