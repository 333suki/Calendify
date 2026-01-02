import { BrowserRouter, Routes, Route } from "react-router-dom";

import Home from "./components/Home/Home.tsx";
import Login from "./components/Login/Login.tsx";
import ProtectedRoutes from "./utils/ProtectedRoutes";
import Register from "./components/Register/Register.tsx";

// import Calendar from "./components/Calendar";
import Events from "./components/Events/Events.tsx";
import EventDetail from "./components/Events/EventDetail.tsx";
import AdminEvents from "./components/Events/AdminEvents.tsx";
import Profile from "./components/Profile/Profile.tsx";
import Settings from "./components/Settings/Settings.tsx";
import RoomBookings from "./components/RoomBookings/RoomBookings.tsx";
import OfficeAttendance from "./components/OfficeAttendance/OfficeAttendance.tsx";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login/>}/>
                <Route path="/register" element={<Register/>}/>
                <Route element={<ProtectedRoutes/>}>
                    <Route path="/home" element={<Home/>} />
                    <Route path="/room-bookings" element={<RoomBookings />} />
                    <Route path="/office-attendance" element={<OfficeAttendance />} />
                    <Route path="/events" element={<Events />} /> 
                    <Route path="/events/:eventId" element={<EventDetail />}/>
                    <Route path="/admin/events" element={<AdminEvents />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/settings" element={<Settings />} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}

export default App;
