import type { Price } from "../../interfaces/Price";
import { LineChart, XAxis, YAxis, Tooltip, CartesianGrid, ResponsiveContainer, Line } from "recharts";

interface Props {
    data: Price[];
}

export default function PriceChart({data}: Props){
    return (
        <div style={{ width: "100%", height: 500 }}>
            <ResponsiveContainer>
                <LineChart data={data}>
                    <CartesianGrid strokeDasharray="3 3"/>
                    <XAxis
                        dataKey="date"
                        type="number"
                        scale="time"
                        domain={["auto", "auto"]}
                        tickFormatter={(unix) =>
                            new Date(unix).toLocaleDateString("tr-TR")
                        }
                    />
                    <YAxis dataKey="rate"/>
                    <Tooltip
                        labelFormatter={(unix) =>
                            new Date(unix).toLocaleString("tr-TR")
                        }
                    />
                    <Line type="monotone" dataKey="rate" stroke="#8884d8" dot={false}/>
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}