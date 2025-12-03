import type { Price } from "../../interfaces/Price";
import { LineChart, XAxis, YAxis, Tooltip, CartesianGrid, ResponsiveContainer, Line } from "recharts";
import { useTheme } from "../../context/ThemeContext";

interface Props {
    data: Price[];
}

export default function PriceChart({data}: Props){
    const { theme } = useTheme();
    const strokeColor = theme === "dark" ? "#4ade80" : "#2563eb";
    const gridColor = theme === "dark" ? "#374151" : "#999";
    const textColor = theme === "dark" ? "#d1d5db" : "#111827";

    const rates = data.map(d => d.rate);
    const min = Math.min(...rates);
    const max = Math.max(...rates);

    // Yuvarlanmış rateler, Y Ekseni domaini için
    const paddedMin = Math.floor(min * 100) / 100;
    const paddedMax = Math.ceil(max * 100) / 100;

    return (
        <div className="w-full h-124">
            <ResponsiveContainer>
                <LineChart data={data}>
                    <CartesianGrid
                        stroke={gridColor}
                        strokeDasharray="3 3"
                    />
                    <XAxis
                        stroke={textColor}
                        dataKey="date"
                        type="number"
                        scale="time"
                        domain={["auto", "auto"]}
                        tickMargin={10}
                        tickFormatter={(unix) =>
                            new Date(unix).toLocaleDateString("tr-TR")
                        }
                    />
                    <YAxis
                        tickMargin={10}
                        domain={[paddedMin, paddedMax]}
                        stroke={textColor}
                        dataKey="rate"
                    />
                    <Tooltip
                        contentStyle={{
                            backgroundColor: theme === "dark" ? "#1f2937" : "#fff",
                            borderColor: theme === "dark" ? "#4b5563" : "#ddd",
                            color: textColor
                        }}
                        labelFormatter={(unix) =>
                            new Date(unix).toLocaleString("tr-TR")
                        }
                    />
                    <Line
                        type="monotone"
                        dataKey="rate"
                        stroke={strokeColor}
                        strokeWidth={2}
                        dot={true}
                    />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}