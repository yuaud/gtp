import type { ReactNode } from "react";

interface PageCardLayoutProps {
    children: ReactNode;
}

const PageCardLayout: React.FC<PageCardLayoutProps> = ({ children }) => {
    return(
        <div className="flex justify-center mt-4 mb-4 px-4">
            <div className="bg-surface rounded-xl shadow-md w-auto p-2">
                {children}
            </div>
        </div>
    );
};

export default PageCardLayout;