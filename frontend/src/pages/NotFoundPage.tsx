import { Link } from "react-router";

const NotFoundPage = () => {
  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-800">
      <div className="text-center">
        <p className="text-5xl font-bold text-white">404 Page Not Found</p>
        <Link to={"/"}>
          <button className="p-2 mt-10 cursor-pointer border-2 border-white rounded-4xl bg-green-500 text-lg font-semibold text-white
            transition transform duration-300 ease-in-out hover:scale-115">
            Return to Home
          </button>
        </Link>
      </div>
    </div>
  );
};

export default NotFoundPage;
