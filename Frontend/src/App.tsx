import { BrowserRouter, Routes, Route } from "react-router-dom";

import Dashboard from "./components/Dashboard";
import Login from "./components/Login";
import ProtectedRoutes from "./utils/ProtectedRoutes";
import Register from "./components/Register";

import Calendar from "./components/Calendar";
import Events from "./components/Events";
import Profile from "./components/Profile";
import Settings from "./components/Settings";
import RoomBookings from "./components/RoomBookings";
import OfficeAttendance from "./components/OfficeAttendance";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login/>}/>
                <Route path="/register" element={<Register/>}/>
                <Route element={<ProtectedRoutes/>}>
                    <Route path="/dashboard" element={<Dashboard/>} />
                    <Route path="/room-bookings" element={<RoomBookings />} />
                    <Route path="/office-attendance" element={<OfficeAttendance />} />
                    <Route path="/calendar" element={<Calendar />} />
                    <Route path="/events" element={<Events />} /> 
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/settings" element={<Settings />} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}

export default App;
