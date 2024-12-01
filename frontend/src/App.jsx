import RealTimeChart from "./components/RealTimeChart";

function App() {
  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">
        IoT Production Line Monitoring
      </h1>
      <RealTimeChart />
    </div>
  );
}

export default App;
