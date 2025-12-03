import type { HTMLAttributes, ReactNode } from "react";

interface PageCardLayoutProps extends HTMLAttributes<HTMLDivElement> {
    children: ReactNode;
    className?: string;
}

const PageCardLayout: React.FC<PageCardLayoutProps> = ({ children, className, ...props }) => {
    return(
        <div className="flex justify-center my-2 px-2">
            <div className={`bg-surface rounded-lg   p-2 ${className ? className : "w-auto" }`}>
                {children}
            </div>
        </div>
    );
};

export default PageCardLayout;