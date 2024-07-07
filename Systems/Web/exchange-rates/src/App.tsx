import './App.css'
import Header from '@components/Header'
import Footer from '@components/Footer'
import MainContent from '@components/MainContent'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AboutPage from '@pages/About';
import VoluteDetailsPage from '@pages/VoluteDetails';
import ComparisonPage from '@pages/Comparison';
import store from './store/store';
import { Provider } from 'react-redux';
import SignInPage from '@pages/SignIn';
import SignUpPage from '@pages/SignUp';
import ProfilePage from '@pages/Profile';

const routes = [
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
  },
  {
    path: "/currency",
    element: <ComparisonPage />
  },
  {
    path: "/signIn",
    element: <SignInPage />
  },
  {
    path: "/signUp",
    element: <SignUpPage />
  },
  {
    path: "/p/:id",
    element: <ProfilePage />
  }
];

const App: React.FC = () => {
  return (
    <>
      <Provider store={store}>
        <Router>
          <header>
            <Header />
          </header>
          <main>
            <Routes>
              {routes.map((route, index) => (
                <Route key={index} path={route.path} element={route.element} />
              ))}
            </Routes>
          </main>
          <footer>
            <Footer />
          </footer>
        </Router>
      </Provider>
    </>
  );
};

export default App;

