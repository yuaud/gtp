import { LineChart, XAxis, YAxis, Tooltip, CartesianGrid, ResponsiveContainer, Line } from "recharts";
import { useTheme } from "../../context/ThemeContext";
import type { PriceMetal } from "../../interfaces/PriceMetal";
import { useTranslation } from "react-i18next";

interface Props {
    data: PriceMetal[];
}

export default function MetalsChart({data}: Props){
    const { theme } = useTheme();
    const { t } = useTranslation();
    const strokeColor = theme === "dark" ? "#4ade80" : "#2563eb";
    const gridColor = theme === "dark" ? "#374151" : "#999";
    const textColor = theme === "dark" ? "#d1d5db" : "#111827";

    const rates = data.map(d => d.gramMetal);
    const min = Math.min(...rates);
    const max = Math.max(...rates);

    // Yuvarlanmış rateler, Y Ekseni domaini için
    const paddedMin = Math.floor(min * 100) / 100;
    const paddedMax = Math.ceil(max * 100) / 100;

    return (
        <div className="w-full h-124">
            <ResponsiveContainer>
                <LineChart margin={{ top: 20, bottom:20, left: 20, right: 20 }} data={data}>
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
                        label={{
                            value: t("chart.date"),
                            angle: 0,
                            position: "insideTopRight",     
                            offset: -15
                        }}
                    />
                    <YAxis
                        tickMargin={10}
                        domain={[paddedMin, paddedMax]}
                        stroke={textColor}
                        dataKey="gramMetal"
                        label={{
                            value: t("chart.price")+" (Gram)",
                            angle: 0,
                            position: "top",     
                            offset: 5
                        }}
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
                        dataKey="gramMetal"
                        stroke={strokeColor}
                        strokeWidth={2}
                        dot={true}
                        name="test"
                    />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}