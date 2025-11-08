import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import NotFoundPage from "./pages/NotFoundPage";
import Layout from "./layout/Layout";

function App() {
  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={<Layout/>}>
            // Anasayfa
            <Route index element={<HomePage />} />
            // 404 Not Found Sayfasi
            <Route path="*" element={<NotFoundPage />} />  
          </Route>
        </Routes>
      </Router>
    </>
  );
}

export default App;
