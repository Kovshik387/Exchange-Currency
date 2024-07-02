import './App.css'
import Header from '@components/Header'
import Footer from '@components/Footer'
import MainContent from '@components/MainContent'
import * as ReactRouter from "react-router-dom";
import AboutPage from '@pages/About';
import VoluteDetailsPage from '@pages/VoluteDetails';

const App: React.FC = () => {

  const router = ReactRouter.createBrowserRouter([
    {
      path: "/daily",
      element: <MainContent />
    },
    {
      path: "/",
      element: <AboutPage />
    },
    {
      path: "/details/:name",
      element: <VoluteDetailsPage />
    }
  ]);

  return (
    <>
      <header>
        <Header />
      </header>
      <main>
        <ReactRouter.RouterProvider router={router} />
      </main>
      <footer>
        <Footer />
      </footer>
    </>
  );
};

export default App
