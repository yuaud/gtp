import { useTranslation } from "react-i18next";
import { Link } from "react-router";

const NotFoundPage = () => {
  const {t, i18n} = useTranslation();
  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-bg">
      <div className="text-center text-text">
        <p className="text-5xl font-bold">{t("notfound_page.not_found")}</p>
        <Link to={"/"}>
          <button className="p-2 mt-10 cursor-pointer border-2 border-white rounded-4xl bg-surface text-lg font-semibold
            transition transform duration-300 ease-in-out hover:scale-110 hover:text-accent">
            {t("notfound_page.return_to_home")}
          </button>
        </Link>
      </div>
    </div>
  );
};

export default NotFoundPage;
