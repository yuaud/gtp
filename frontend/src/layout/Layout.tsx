import Navbar from '../components/navbar/Navbar';
import Footer from '../components/Footer';
import { Outlet } from 'react-router';

const Layout = () => {
  return (
    <div className='flex flex-col min-h-screen'>
        <Navbar/>
        <main className='flex-1'>
            <Outlet/>
        </main>
        <Footer/>
    </div>
  )
}

export default Layout