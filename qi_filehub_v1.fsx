import { useState, useEffect, useRef, useCallback } from "react";

/* ─────────────────────────────────────────────
   GLOBAL STYLES  — Premium Black / Silver
───────────────────────────────────────────── */
const CSS = `
@import url('https://fonts.googleapis.com/css2?family=Instrument+Serif:ital@0;1&family=Geist:wght@300;400;500;600;700&family=Geist+Mono:wght@400;500&display=swap');

*, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

:root {
  --bg:        #050505;
  --bg1:       #0A0A0A;
  --bg2:       #0E0E0E;
  --bg3:       #141414;
  --bg4:       #1A1A1A;
  --border:    #2B2B2B;
  --border2:   #383838;
  --silver:    #C0C0C0;
  --silver2:   #D9D9D9;
  --silver3:   #F0F0F0;
  --white:     #FFFFFF;
  --muted:     #888888;
  --muted2:    #606060;
  --muted3:    #404040;
  --glow:      rgba(192,192,192,0.08);
  --glowHover: rgba(192,192,192,0.14);
  --glowStr:   rgba(192,192,192,0.25);
  --r:         10px;
  --r2:        16px;
  --r3:        22px;
  font-family: 'Geist', sans-serif;
  color-scheme: dark;
}

html, body { background: var(--bg); color: var(--silver2); min-height: 100vh; overflow-x: hidden; }
* { -webkit-font-smoothing: antialiased; }

/* Scrollbar */
::-webkit-scrollbar { width: 4px; height: 4px; }
::-webkit-scrollbar-track { background: var(--bg1); }
::-webkit-scrollbar-thumb { background: var(--bg4); border-radius: 2px; }
::-webkit-scrollbar-thumb:hover { background: var(--border2); }

/* Keyframes */
@keyframes fadeUp   { from { opacity:0; transform:translateY(16px) } to { opacity:1; transform:translateY(0) } }
@keyframes fadeIn   { from { opacity:0 } to { opacity:1 } }
@keyframes shimmer  { 0%{ background-position:-400px 0 } 100%{ background-position:400px 0 } }
@keyframes pulse    { 0%,100%{ opacity:1 } 50%{ opacity:.4 } }
@keyframes scanline { 0%{ transform:translateY(-100%) } 100%{ transform:translateY(100vh) } }
@keyframes glow     { 0%,100%{ opacity:.3 } 50%{ opacity:.7 } }
@keyframes spin     { to { transform:rotate(360deg) } }
@keyframes float    { 0%,100%{ transform:translateY(0) } 50%{ transform:translateY(-6px) } }
@keyframes slideIn  { from { transform:translateX(-100%); opacity:0 } to { transform:translateX(0); opacity:1 } }
@keyframes modalIn  { from { opacity:0; transform:scale(.96) translateY(8px) } to { opacity:1; transform:scale(1) translateY(0) } }

/* Page reveal */
.page { animation: fadeUp .35s cubic-bezier(.22,1,.36,1) forwards; }

/* ── Layout ── */
.app { display:flex; min-height:100vh; }
.main-area { flex:1; display:flex; flex-direction:column; min-width:0; overflow-x:hidden; }

/* ── Sidebar ── */
.sidebar {
  width:240px; min-width:240px; height:100vh; position:sticky; top:0;
  background:var(--bg1); border-right:1px solid var(--border);
  display:flex; flex-direction:column; overflow-y:auto; overflow-x:hidden;
  transition:width .3s cubic-bezier(.22,1,.36,1), min-width .3s;
  z-index:200;
}
.sidebar.collapsed { width:64px; min-width:64px; }
.sidebar.collapsed .sl-label, .sidebar.collapsed .sl-badge,
.sidebar.collapsed .sidebar-logo-text, .sidebar.collapsed .sidebar-user-info,
.sidebar.collapsed .sidebar-section-label { opacity:0; pointer-events:none; }
.sidebar.collapsed .sidebar-user { justify-content:center; }

.sidebar-logo { display:flex; align-items:center; gap:12px; padding:22px 18px 18px; border-bottom:1px solid var(--border); cursor:pointer; }
.sidebar-logo-icon { width:32px; height:32px; background: linear-gradient(135deg,var(--silver),var(--white)); border-radius:8px; display:flex; align-items:center; justify-content:center; flex-shrink:0; }
.sidebar-logo-icon svg { width:18px; height:18px; }
.sidebar-logo-text { font-family:'Instrument Serif',serif; font-size:18px; color:var(--white); letter-spacing:-.3px; white-space:nowrap; overflow:hidden; transition:opacity .2s; }
.sidebar-logo-text span { color:var(--silver); }

.sidebar-section-label { font-size:10px; font-weight:600; letter-spacing:1.2px; color:var(--muted3); text-transform:uppercase; padding:16px 18px 6px; white-space:nowrap; transition:opacity .2s; }

.sl {
  display:flex; align-items:center; gap:12px; padding:9px 14px; margin:1px 8px;
  border-radius:var(--r); color:var(--muted); font-size:13.5px; font-weight:400;
  cursor:pointer; transition:all .18s ease; white-space:nowrap; position:relative;
}
.sl:hover { background:var(--glow); color:var(--silver2); }
.sl.active { background:var(--glowHover); color:var(--white); }
.sl.active .sl-icon { color:var(--silver); }
.sl-icon { width:18px; height:18px; flex-shrink:0; display:flex; align-items:center; justify-content:center; }
.sl-label { transition:opacity .2s; overflow:hidden; }
.sl-badge { margin-left:auto; background:var(--border2); color:var(--silver); font-size:10px; font-weight:600; padding:2px 7px; border-radius:20px; transition:opacity .2s; }

.sidebar-user {
  margin-top:auto; padding:12px 10px; border-top:1px solid var(--border);
  display:flex; align-items:center; gap:10px; cursor:pointer;
  transition:background .18s; border-radius:0 0 0 0;
}
.sidebar-user:hover { background:var(--glow); }
.sidebar-user-av { width:34px; height:34px; border-radius:50%; background:var(--bg3); border:1.5px solid var(--border2); display:flex; align-items:center; justify-content:center; font-size:13px; font-weight:600; color:var(--silver); flex-shrink:0; font-family:'Geist',sans-serif; }
.sidebar-user-info { overflow:hidden; transition:opacity .2s; }
.sidebar-user-name { font-size:13px; font-weight:500; color:var(--white); overflow:hidden; text-overflow:ellipsis; white-space:nowrap; }
.sidebar-user-handle { font-size:11px; color:var(--muted); }

/* Hamburger */
.hamburger { display:flex; align-items:center; justify-content:center; width:32px; height:32px; border-radius:8px; cursor:pointer; border:1px solid var(--border); background:var(--bg2); transition:all .18s; }
.hamburger:hover { border-color:var(--border2); background:var(--bg3); }

/* ── Topbar ── */
.topbar {
  height:56px; background:rgba(5,5,5,.85); backdrop-filter:blur(20px);
  border-bottom:1px solid var(--border); display:flex; align-items:center;
  justify-content:space-between; padding:0 24px; position:sticky; top:0; z-index:100;
}

/* ── Content ── */
.content { flex:1; padding:28px 32px; width:100%; }
.content-wide { flex:1; padding:28px 32px; }

/* ── Cards ── */
.card {
  background:var(--bg2); border:1px solid var(--border); border-radius:var(--r2);
  transition:border-color .2s, box-shadow .2s;
}
.card:hover { border-color:var(--border2); box-shadow:0 0 0 1px var(--glow), 0 8px 32px rgba(0,0,0,.5); }
.card-glass {
  background:rgba(14,14,14,.7); border:1px solid var(--border); border-radius:var(--r2);
  backdrop-filter:blur(16px);
}
.card-inset { background:var(--bg3); border:1px solid var(--border); border-radius:var(--r); }

/* ── Buttons ── */
.btn {
  display:inline-flex; align-items:center; gap:7px;
  padding:9px 18px; border-radius:var(--r); font-family:'Geist',sans-serif;
  font-size:13.5px; font-weight:500; cursor:pointer; border:none;
  transition:all .18s ease; white-space:nowrap;
}
.btn-sm { padding:6px 14px; font-size:12.5px; border-radius:8px; gap:5px; }
.btn-lg { padding:12px 24px; font-size:15px; border-radius:12px; }
.btn-primary {
  background:var(--white); color:#050505;
  box-shadow:0 0 0 0 rgba(255,255,255,0);
}
.btn-primary:hover { background:var(--silver3); transform:translateY(-1px); box-shadow:0 4px 20px rgba(255,255,255,.1); }
.btn-primary:active { transform:translateY(0); }
.btn-secondary {
  background:var(--bg3); color:var(--silver2); border:1px solid var(--border);
}
.btn-secondary:hover { border-color:var(--border2); background:var(--bg4); color:var(--white); }
.btn-ghost { background:transparent; color:var(--muted); }
.btn-ghost:hover { color:var(--silver2); background:var(--glow); }
.btn-outline { background:transparent; color:var(--silver); border:1px solid var(--border2); }
.btn-outline:hover { border-color:var(--silver); color:var(--white); background:var(--glow); }
.btn-danger { background:rgba(239,68,68,.1); color:#ef4444; border:1px solid rgba(239,68,68,.2); }
.btn-danger:hover { background:rgba(239,68,68,.18); }

/* ── Inputs ── */
.input {
  width:100%; padding:10px 14px; border-radius:var(--r);
  background:var(--bg3); border:1px solid var(--border);
  color:var(--silver2); font-family:'Geist',sans-serif; font-size:13.5px;
  transition:border-color .18s, box-shadow .18s; outline:none;
}
.input:focus { border-color:var(--border2); box-shadow:0 0 0 3px rgba(192,192,192,.06); color:var(--white); }
.input::placeholder { color:var(--muted3); }
.input-label { font-size:12px; color:var(--muted); margin-bottom:6px; display:block; font-weight:500; letter-spacing:.2px; }
.textarea { resize:vertical; min-height:90px; line-height:1.6; }
.select { appearance:none; cursor:pointer; }

/* ── Tags / Badges ── */
.tag { display:inline-flex; align-items:center; padding:3px 9px; border-radius:6px; font-size:11.5px; font-weight:500; background:var(--bg4); color:var(--muted); border:1px solid var(--border); }
.tag-silver { background:rgba(192,192,192,.07); color:var(--silver); border-color:rgba(192,192,192,.15); }
.tag-white { background:rgba(255,255,255,.06); color:var(--white); border-color:rgba(255,255,255,.12); }
.badge { display:inline-flex; align-items:center; padding:2px 8px; border-radius:20px; font-size:11px; font-weight:600; }
.badge-new { background:rgba(192,192,192,.1); color:var(--silver); border:1px solid rgba(192,192,192,.2); }

/* ── Avatar ── */
.av {
  border-radius:50%; display:flex; align-items:center; justify-content:center;
  font-family:'Geist',sans-serif; font-weight:600; color:var(--silver);
  background:var(--bg3); border:1.5px solid var(--border2); flex-shrink:0;
  font-size:13px;
}

/* ── Dividers ── */
.divider { height:1px; background:var(--border); }
.divider-v { width:1px; background:var(--border); align-self:stretch; }
.shine-line { height:1px; background:linear-gradient(90deg,transparent,var(--border2),transparent); }

/* ── File Explorer ── */
.file-tree { font-family:'Geist Mono',monospace; font-size:12.5px; color:var(--muted); }
.ft-row {
  display:flex; align-items:center; gap:7px; padding:5px 8px; border-radius:6px;
  cursor:pointer; transition:background .15s; user-select:none;
}
.ft-row:hover { background:var(--glow); color:var(--silver2); }
.ft-row.active { background:var(--glowHover); color:var(--white); }
.ft-indent { border-left:1px solid var(--border); margin-left:12px; }

/* ── Code viewer ── */
.code-view { font-family:'Geist Mono',monospace; font-size:12px; line-height:1.7; color:var(--silver2); }
.code-line { display:flex; gap:0; }
.code-ln { color:var(--muted3); text-align:right; padding:0 14px 0 8px; min-width:44px; user-select:none; border-right:1px solid var(--border); margin-right:14px; }

/* ── Thumbnail cards ── */
.proj-card {
  background:var(--bg2); border:1px solid var(--border); border-radius:var(--r2);
  overflow:hidden; cursor:pointer; transition:all .22s;
}
.proj-card:hover { border-color:var(--border2); transform:translateY(-3px); box-shadow:0 12px 40px rgba(0,0,0,.6), 0 0 0 1px var(--glow); }
.proj-thumb {
  width:100%; aspect-ratio:16/9; object-fit:cover;
  background:var(--bg3); display:flex; align-items:center; justify-content:center;
  border-bottom:1px solid var(--border);
}
.proj-thumb-placeholder { width:100%; aspect-ratio:16/9; background:var(--bg3); border-bottom:1px solid var(--border); display:flex; align-items:center; justify-content:center; }

/* ── Empty State ── */
.empty {
  display:flex; flex-direction:column; align-items:center; justify-content:center;
  padding:64px 32px; text-align:center; gap:16px; color:var(--muted);
}
.empty-icon { width:56px; height:56px; border:1px solid var(--border); border-radius:16px; display:flex; align-items:center; justify-content:center; margin-bottom:8px; }

/* ── Modal ── */
.modal-overlay { position:fixed; inset:0; background:rgba(0,0,0,.7); backdrop-filter:blur(8px); z-index:1000; display:flex; align-items:center; justify-content:center; padding:20px; animation:fadeIn .2s ease; }
.modal { background:var(--bg2); border:1px solid var(--border); border-radius:var(--r3); padding:32px; width:100%; max-width:520px; max-height:90vh; overflow-y:auto; animation:modalIn .25s cubic-bezier(.22,1,.36,1); }
.modal-lg { max-width:760px; }

/* ── Scan line effect on hero ── */
.scan-effect { position:absolute; inset:0; pointer-events:none; overflow:hidden; }
.scan-line { position:absolute; left:0; right:0; height:1px; background:linear-gradient(90deg,transparent,rgba(192,192,192,.06),transparent); animation:scanline 6s linear infinite; }

/* ── Tooltip ── */
.tooltip-wrap { position:relative; display:inline-flex; }
.tooltip-wrap:hover .tooltip { opacity:1; pointer-events:auto; }
.tooltip { position:absolute; bottom:calc(100% + 6px); left:50%; transform:translateX(-50%); background:var(--bg4); border:1px solid var(--border2); color:var(--silver2); font-size:11.5px; padding:5px 10px; border-radius:6px; white-space:nowrap; opacity:0; pointer-events:none; transition:opacity .15s; z-index:999; }

/* ── Notification dot ── */
.ndot { width:6px; height:6px; border-radius:50%; background:var(--silver); animation:glow 2s infinite; }

/* Utility */
.flex { display:flex; } .flex-col { flex-direction:column; } .items-center { align-items:center; } .justify-between { justify-content:space-between; } .justify-center { justify-content:center; }
.gap-1{gap:4px} .gap-2{gap:8px} .gap-3{gap:12px} .gap-4{gap:16px} .gap-5{gap:20px} .gap-6{gap:24px}
.w-full{width:100%} .h-full{height:100%} .flex-1{flex:1} .min-w-0{min-width:0}
.text-white{color:var(--white)} .text-silver{color:var(--silver)} .text-silver2{color:var(--silver2)} .text-muted{color:var(--muted)} .text-muted2{color:var(--muted2)}
.text-xs{font-size:11.5px} .text-sm{font-size:13px} .text-base{font-size:14px} .text-lg{font-size:17px} .text-xl{font-size:22px} .text-2xl{font-size:28px} .text-3xl{font-size:36px} .text-4xl{font-size:48px} .text-5xl{font-size:60px}
.font-medium{font-weight:500} .font-semibold{font-weight:600} .font-bold{font-weight:700}
.font-serif{font-family:'Instrument Serif',serif}
.font-mono{font-family:'Geist Mono',monospace}
.leading-tight{line-height:1.2} .leading-relaxed{line-height:1.7} .leading-loose{line-height:2}
.truncate{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.relative{position:relative} .overflow-hidden{overflow:hidden}
.p-0{padding:0} .p-3{padding:12px} .p-4{padding:16px} .p-5{padding:20px} .p-6{padding:24px} .p-8{padding:32px}
.px-4{padding-left:16px;padding-right:16px} .px-6{padding-left:24px;padding-right:24px}
.py-2{padding-top:8px;padding-bottom:8px} .py-3{padding-top:12px;padding-bottom:12px}
.mt-auto{margin-top:auto}
.grid-2{display:grid;grid-template-columns:repeat(2,1fr);gap:20px}
.grid-3{display:grid;grid-template-columns:repeat(3,1fr);gap:20px}
.grid-4{display:grid;grid-template-columns:repeat(4,1fr);gap:16px}
@media(max-width:1100px){.grid-4{grid-template-columns:repeat(2,1fr)}.grid-3{grid-template-columns:repeat(2,1fr)}}
@media(max-width:720px){.grid-2,.grid-3,.grid-4{grid-template-columns:1fr}.sidebar{position:fixed;left:0;top:0;height:100vh;z-index:300}}

/* Upload drop zone */
.dropzone {
  border:1.5px dashed var(--border2); border-radius:var(--r2);
  padding:48px 32px; text-align:center; cursor:pointer;
  transition:all .2s; background:var(--bg3);
}
.dropzone:hover,.dropzone.drag { border-color:var(--silver); background:rgba(192,192,192,.03); }
.dropzone input { display:none; }

/* Search highlight */
.hl { background:rgba(192,192,192,.15); border-radius:3px; padding:0 2px; }

/* Profile cover */
.profile-cover { width:100%; height:200px; background:var(--bg3); border-bottom:1px solid var(--border); position:relative; overflow:hidden; }
.cover-pattern {
  position:absolute; inset:0;
  background:
    repeating-linear-gradient(0deg,transparent,transparent 39px,var(--border) 39px,var(--border) 40px),
    repeating-linear-gradient(90deg,transparent,transparent 39px,var(--border) 39px,var(--border) 40px);
  opacity:.4;
}

/* Tab system */
.tabs { display:flex; gap:0; border-bottom:1px solid var(--border); }
.tab { padding:10px 20px; font-size:13px; font-weight:500; color:var(--muted); cursor:pointer; border-bottom:2px solid transparent; margin-bottom:-1px; transition:all .18s; }
.tab:hover { color:var(--silver2); }
.tab.active { color:var(--white); border-bottom-color:var(--silver); }

/* Stat */
.stat { text-align:center; }
.stat-val { font-family:'Instrument Serif',serif; font-size:22px; color:var(--white); line-height:1.1; }
.stat-lbl { font-size:11px; color:var(--muted); text-transform:uppercase; letter-spacing:.8px; margin-top:3px; }

/* Progress */
.progress { height:3px; background:var(--bg4); border-radius:2px; overflow:hidden; }
.progress-bar { height:100%; background:linear-gradient(90deg,var(--muted2),var(--silver)); transition:width .5s cubic-bezier(.22,1,.36,1); }

/* Toast */
.toast { position:fixed; bottom:24px; right:24px; background:var(--bg3); border:1px solid var(--border2); border-radius:var(--r); padding:12px 18px; font-size:13.5px; color:var(--silver2); z-index:9999; display:flex; align-items:center; gap:10px; box-shadow:0 8px 40px rgba(0,0,0,.5); animation:fadeUp .3s ease; }

/* Chat bubble */
.bubble { max-width:70%; padding:10px 14px; border-radius:14px; font-size:13.5px; line-height:1.6; }
.bubble-me { background:var(--bg4); color:var(--silver2); border-bottom-right-radius:4px; }
.bubble-them { background:var(--bg3); border:1px solid var(--border); color:var(--silver2); border-bottom-left-radius:4px; }
`;

/* ─── SVG Icons ─────────────────────────────── */
const Icon = {
  Logo: () => <svg viewBox="0 0 20 20" fill="none"><path d="M3 4h7l6 12H3V4z" stroke="#050505" strokeWidth="1.5" fill="none"/><circle cx="14" cy="8" r="3" fill="#050505"/></svg>,
  Home: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M2 8l7-6 7 6v8H2V8z"/><path d="M7 14v-4h4v4"/></svg>,
  Explore: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><circle cx="9" cy="9" r="7"/><path d="M11.5 6.5l-2.5 5-2.5-2.5 5-2.5z"/></svg>,
  Upload: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M3 12v3h12v-3M9 3v9M5 7l4-4 4 4"/></svg>,
  Profile: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><circle cx="9" cy="6" r="3"/><path d="M3 16c0-3.314 2.686-5 6-5s6 1.686 6 5"/></svg>,
  Bell: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M9 2a6 6 0 016 6v3l1 2H2l1-2V8a6 6 0 016-6z"/><path d="M7 15a2 2 0 004 0"/></svg>,
  Message: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M2 4h14v9H2z" rx="2"/><path d="M6 9l3 3 3-3"/></svg>,
  Settings: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><circle cx="9" cy="9" r="2.5"/><path d="M9 1v2M9 15v2M1 9h2M15 9h2M3.22 3.22l1.41 1.41M13.37 13.37l1.41 1.41M3.22 14.78l1.41-1.41M13.37 4.63l1.41-1.41"/></svg>,
  Search: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><circle cx="8" cy="8" r="5.5"/><path d="M14 14l-2-2"/></svg>,
  Folder: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.3"><path d="M1 4h5l1 1.5h7V12H1V4z"/></svg>,
  FolderOpen: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.3"><path d="M1 5h5l1 1.5h7l-1.5 6H1.5L1 5z"/></svg>,
  File: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.3"><path d="M3 2h7l3 3v9H3V2z"/><path d="M10 2v3h3"/></svg>,
  ChevRight: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M6 3l5 5-5 5"/></svg>,
  ChevDown: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M3 6l5 5 5-5"/></svg>,
  ChevLeft: () => <svg viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M10 3L5 8l5 5"/></svg>,
  Download: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M9 3v9M5 8l4 4 4-4"/><path d="M3 15h12"/></svg>,
  Heart: ({filled}) => <svg viewBox="0 0 18 18" fill={filled?"currentColor":"none"} stroke="currentColor" strokeWidth="1.5"><path d="M9 15s-7-4.5-7-9a4 4 0 018 0 4 4 0 018 0c0 4.5-7 9-7 9z"/></svg>,
  Star: ({filled}) => <svg viewBox="0 0 18 18" fill={filled?"currentColor":"none"} stroke="currentColor" strokeWidth="1.5"><path d="M9 2l2 5h5l-4 3 1.5 5L9 12.5 4.5 15 6 10 2 7h5z"/></svg>,
  Copy: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><rect x="6" y="6" width="9" height="9" rx="1"/><path d="M3 12V3h9"/></svg>,
  X: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M3 3l12 12M15 3L3 15"/></svg>,
  Plus: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M9 3v12M3 9h12"/></svg>,
  Send: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M16 2L2 7l6 4 5-5-5 5 1 6 7-15z"/></svg>,
  Grid: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><rect x="2" y="2" width="6" height="6" rx="1"/><rect x="10" y="2" width="6" height="6" rx="1"/><rect x="2" y="10" width="6" height="6" rx="1"/><rect x="10" y="10" width="6" height="6" rx="1"/></svg>,
  Menu: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M2 5h14M2 9h14M2 13h14"/></svg>,
  External: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M10 3h5v5M15 3L8 10M7 5H4a2 2 0 00-2 2v7a2 2 0 002 2h7a2 2 0 002-2v-3"/></svg>,
  GitHub: () => <svg viewBox="0 0 18 18" fill="currentColor"><path d="M9 1a8 8 0 00-2.53 15.59c.4.07.55-.17.55-.38v-1.33C4.73 15.4 4.26 13.8 4.26 13.8c-.36-.92-.88-1.17-.88-1.17-.72-.49.05-.48.05-.48.8.06 1.22.82 1.22.82.71 1.21 1.86.86 2.31.66.07-.52.28-.87.5-1.07-1.77-.2-3.63-.88-3.63-3.93 0-.87.31-1.58.82-2.13-.08-.2-.36-1.01.08-2.1 0 0 .67-.22 2.2.82a7.65 7.65 0 014 0c1.52-1.04 2.2-.82 2.2-.82.44 1.09.16 1.9.08 2.1.51.55.82 1.26.82 2.13 0 3.06-1.86 3.73-3.64 3.93.29.25.54.73.54 1.48v2.19c0 .21.14.46.55.38A8 8 0 009 1z"/></svg>,
  Trending: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M2 12l4-4 3 3 5-5"/><path d="M12 6h3v3"/></svg>,
  Admin: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M9 2l1.5 3H14l-2.5 2 1 3L9 8.5 6.5 10l1-3L5 5h3.5z"/><circle cx="9" cy="14" r="2"/></svg>,
  Bookmark: ({filled}) => <svg viewBox="0 0 18 18" fill={filled?"currentColor":"none"} stroke="currentColor" strokeWidth="1.5"><path d="M4 2h10v14l-5-3.5L4 16V2z"/></svg>,
  Zap: () => <svg viewBox="0 0 18 18" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M11 2L5 10h6l-2 6 6-8h-6z"/></svg>,
};

/* ─── FILE TREE DEMO DATA ───────────────────── */
const DEMO_FILE_TREE = [
  { id:"src", name:"src", type:"dir", children:[
    { id:"components", name:"components", type:"dir", children:[
      { id:"App.jsx", name:"App.jsx", type:"file", ext:"jsx", size:"3.2 KB" },
      { id:"Navbar.jsx", name:"Navbar.jsx", type:"file", ext:"jsx", size:"1.8 KB" },
    ]},
    { id:"pages", name:"pages", type:"dir", children:[
      { id:"Home.jsx", name:"Home.jsx", type:"file", ext:"jsx", size:"5.4 KB" },
      { id:"Profile.jsx", name:"Profile.jsx", type:"file", ext:"jsx", size:"4.1 KB" },
    ]},
    { id:"main.jsx", name:"main.jsx", type:"file", ext:"jsx", size:"0.8 KB" },
    { id:"index.css", name:"index.css", type:"file", ext:"css", size:"12.3 KB" },
  ]},
  { id:"public", name:"public", type:"dir", children:[
    { id:"index.html", name:"index.html", type:"file", ext:"html", size:"1.1 KB" },
    { id:"favicon.svg", name:"favicon.svg", type:"file", ext:"svg", size:"2.4 KB" },
  ]},
  { id:"package.json", name:"package.json", type:"file", ext:"json", size:"1.2 KB" },
  { id:"README.md", name:"README.md", type:"file", ext:"md", size:"3.8 KB" },
  { id:"vite.config.js", name:"vite.config.js", type:"file", ext:"js", size:"0.5 KB" },
];

const DEMO_FILE_CONTENT = {
  "App.jsx": `import { useState } from "react";
import { Router } from "./Router";
import { ThemeProvider } from "./context/theme";
import Navbar from "./components/Navbar";
import "./index.css";

export default function App() {
  const [theme, setTheme] = useState("dark");
  
  return (
    <ThemeProvider value={{ theme, setTheme }}>
      <div className="app">
        <Navbar />
        <Router />
      </div>
    </ThemeProvider>
  );
}`,
  "README.md": `# My Awesome Project

A modern full-stack web application built with React and FastAPI.

## Features

- Real-time updates with WebSockets
- JWT authentication
- File upload system
- Responsive design

## Getting Started

\`\`\`bash
npm install
npm run dev
\`\`\`

## Tech Stack

- React 18 + Vite
- FastAPI + Python 3.11
- PostgreSQL
- Tailwind CSS`,
  "package.json": `{
  "name": "my-awesome-project",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview"
  },
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.11.0"
  },
  "devDependencies": {
    "vite": "^4.3.9",
    "@vitejs/plugin-react": "^4.0.0",
    "tailwindcss": "^3.3.2"
  }
}`,
};

/* ─── CATEGORIES ─────────────────────────────── */
const CATEGORIES = [
  { id:"web", name:"Web Development", icon:<Icon.External/> },
  { id:"ai", name:"Artificial Intelligence", icon:<Icon.Zap/> },
  { id:"mobile", name:"Mobile Apps", icon:<Icon.Grid/> },
  { id:"game", name:"Game Development", icon:<Icon.Star filled={false}/> },
  { id:"blender", name:"Blender & 3D Art", icon:<Icon.File/> },
  { id:"robotics", name:"Robotics", icon:<Icon.Settings/> },
  { id:"python", name:"Python Projects", icon:<Icon.File/> },
  { id:"research", name:"Research", icon:<Icon.Search/> },
];

/* ─── EXT COLOR MAP ─────────────────────────── */
const EXT_COLOR = { jsx:"#61dafb", tsx:"#61dafb", js:"#f7df1e", ts:"#3178c6", py:"#3776ab", css:"#264de4", html:"#e34c26", json:"#c0c0c0", md:"#ffffff", txt:"#c0c0c0", cpp:"#004482", java:"#f89820", c:"#a8b9cc", svg:"#ffb13b", rs:"#dea584" };
const extColor = (ext) => EXT_COLOR[ext?.toLowerCase()] || "#888";

/* ─── HELPERS ────────────────────────────────── */
const relTime = (d) => {
  const s = Math.floor((Date.now() - d) / 1000);
  if (s < 60) return `${s}s ago`;
  if (s < 3600) return `${Math.floor(s/60)}m ago`;
  if (s < 86400) return `${Math.floor(s/3600)}h ago`;
  return `${Math.floor(s/86400)}d ago`;
};

const fmtNum = (n) => n >= 1000 ? `${(n/1000).toFixed(1)}k` : String(n);

/* ─── CONTEXT (app state) ─────────────────────── */
const AppCtx = ({ children }) => children;

/* ─── COMPONENT: ThumbPlaceholder ────────────── */
const ThumbPlaceholder = ({ category, size = "full" }) => {
  const colors = { web:"#1a2a1a", ai:"#1a1a2a", mobile:"#2a1a2a", game:"#2a1a1a", blender:"#1a2a2a", default:"#141414" };
  const bg = colors[category] || colors.default;
  return (
    <div style={{ width:"100%", aspectRatio:"16/9", background:bg, borderBottom:"1px solid var(--border)", display:"flex", alignItems:"center", justifyContent:"center", position:"relative", overflow:"hidden" }}>
      <div style={{ position:"absolute", inset:0, backgroundImage:"radial-gradient(circle at 50% 50%, rgba(192,192,192,.06) 0%, transparent 70%)" }}/>
      <div style={{ fontSize:32, opacity:.25 }}>◈</div>
    </div>
  );
};

/* ─── COMPONENT: Avatar ──────────────────────── */
const Av = ({ name="?", size=32, glow=false }) => {
  const init = name?.slice(0,1).toUpperCase() || "?";
  return (
    <div className="av" style={{ width:size, height:size, fontSize:size*0.38, boxShadow:glow?"0 0 12px rgba(192,192,192,.2)":"none" }}>
      {init}
    </div>
  );
};

/* ─── COMPONENT: Toast ───────────────────────── */
let _toast;
const Toast = () => {
  const [msg,setMsg]=useState(null);
  _toast = setMsg;
  useEffect(()=>{if(msg){const t=setTimeout(()=>setMsg(null),2800);return()=>clearTimeout(t);}}, [msg]);
  if(!msg) return null;
  return <div className="toast"><span style={{opacity:.6}}>✦</span>{msg}</div>;
};
const toast = (m) => _toast?.(m);

/* ─── FILE TREE COMPONENT ────────────────────── */
const FileTree = ({ nodes, depth=0, onSelect, selected }) => {
  const [open, setOpen] = useState({});
  return (
    <div className="file-tree" style={{ paddingLeft: depth ? 10 : 0 }}>
      {nodes.map(node => (
        <div key={node.id}>
          <div
            className={`ft-row ${selected===node.id?"active":""}`}
            style={{ paddingLeft: 8 + depth*8 }}
            onClick={() => {
              if(node.type==="dir") setOpen(o=>({...o,[node.id]:!o[node.id]}));
              else onSelect?.(node);
            }}
          >
            <span style={{ opacity:.5, width:12, display:"inline-flex", alignItems:"center" }}>
              {node.type==="dir"
                ? (open[node.id] ? <Icon.ChevDown/> : <Icon.ChevRight/>)
                : null}
            </span>
            <span style={{ color: node.type==="dir" ? "var(--silver2)" : extColor(node.ext), opacity:.8, width:14, display:"inline-flex", alignItems:"center", flexShrink:0 }}>
              {node.type==="dir" ? (open[node.id]?<Icon.FolderOpen/>:<Icon.Folder/>) : <Icon.File/>}
            </span>
            <span style={{ color: node.type==="dir" ? "var(--silver2)" : "var(--muted)", marginLeft:4 }}>{node.name}</span>
            {node.ext && <span style={{ marginLeft:"auto", fontSize:10, color:extColor(node.ext), opacity:.6 }}>.{node.ext}</span>}
          </div>
          {node.type==="dir" && open[node.id] && node.children &&
            <FileTree nodes={node.children} depth={depth+1} onSelect={onSelect} selected={selected}/>}
        </div>
      ))}
    </div>
  );
};

/* ─── COMPONENT: CodeViewer ──────────────────── */
const CodeViewer = ({ file, onClose }) => {
  const content = DEMO_FILE_CONTENT[file?.name] || `// ${file?.name}\n// File content preview\n// (Backend serves real file content)`;
  const lines = content.split("\n");
  const [copied, setCopied] = useState(false);
  return (
    <div style={{ height:"100%", display:"flex", flexDirection:"column" }}>
      <div className="flex items-center justify-between" style={{ padding:"10px 16px", borderBottom:"1px solid var(--border)", background:  "var(--bg1)" }}>
        <div className="flex items-center gap-2">
          <span style={{ color: extColor(file?.ext), opacity:.8, display:"flex" }}><Icon.File/></span>
          <span style={{ fontFamily:"'Geist Mono',monospace", fontSize:12.5 }}>{file?.name}</span>
          {file?.ext && <span className="tag" style={{ fontSize:10, padding:"1px 6px", color:extColor(file?.ext), borderColor: extColor(file?.ext)+"30", background:extColor(file?.ext)+"10" }}>.{file.ext}</span>}
        </div>
        <div className="flex gap-2">
          <button className="btn btn-sm btn-ghost" onClick={() => { setCopied(true); setTimeout(()=>setCopied(false),1500); toast("Copied to clipboard"); }}>
            <Icon.Copy/> {copied?"Copied":"Copy"}
          </button>
          {onClose && <button className="btn btn-sm btn-ghost" onClick={onClose}><Icon.X/></button>}
        </div>
      </div>
      <div style={{ flex:1, overflowY:"auto", background:  "var(--bg1)" }}>
        <div className="code-view" style={{ padding:"12px 0" }}>
          {lines.map((line,i) => (
            <div key={i} className="code-line">
              <span className="code-ln">{i+1}</span>
              <span style={{ flex:1, paddingRight:16 }}>{line||" "}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

/* ─── COMPONENT: ProjectCard ─────────────────── */
const ProjectCard = ({ project, onClick }) => {
  const [liked, setLiked] = useState(false);
  const [saved, setSaved] = useState(false);
  return (
    <div className="proj-card" onClick={()=>onClick?.(project)}>
      {project.thumbnail
        ? <img src={project.thumbnail} alt={project.name} className="proj-thumb" style={{ width:"100%", aspectRatio:"16/9", objectFit:"cover", borderBottom:"1px solid var(--border)" }}/>
        : <ThumbPlaceholder category={project.category}/>}
      <div style={{ padding:"14px 16px" }}>
        <div className="flex items-center justify-between" style={{ marginBottom:8 }}>
          <span className="tag tag-silver" style={{ fontSize:10.5 }}>{project.category||"General"}</span>
          <span style={{ fontSize:11, color:"var(--muted2)" }}>{relTime(project.createdAt||Date.now()-8e7)}</span>
        </div>
        <h3 style={{ fontSize:14.5, fontWeight:600, color:"var(--white)", marginBottom:5, lineHeight:1.3 }}>{project.name}</h3>
        <p style={{ fontSize:12.5, color:"var(--muted)", lineHeight:1.5, marginBottom:10, overflow:"hidden", display:"-webkit-box", WebkitLineClamp:2, WebkitBoxOrient:"vertical" }}>{project.description||"No description provided."}</p>
        <div className="flex" style={{ gap:6, flexWrap:"wrap", marginBottom:12 }}>
          {(project.tags||[]).slice(0,3).map(t=><span key={t} className="tag" style={{ fontSize:10.5 }}>{t}</span>)}
        </div>
        <div className="shine-line" style={{ marginBottom:12 }}/>
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Av name={project.owner||project.author} size={22}/>
            <span style={{ fontSize:12, color:"var(--muted)" }}>@{project.owner||project.author||"user"}</span>
          </div>
          <div className="flex items-center gap-3" style={{ fontSize:12, color:"var(--muted2)" }}>
            <button className={`btn btn-ghost`} style={{ padding:"2px 6px", fontSize:11, gap:4, color:liked?"var(--silver)":"var(--muted2)" }}
              onClick={e=>{e.stopPropagation();setLiked(!liked);}}>
              <Icon.Heart filled={liked}/> {fmtNum((project.likes||0)+(liked?1:0))}
            </button>
            <button className={`btn btn-ghost`} style={{ padding:"2px 6px", fontSize:11, color:saved?"var(--silver)":"var(--muted2)" }}
              onClick={e=>{e.stopPropagation();setSaved(!saved);}}>
              <Icon.Bookmark filled={saved}/>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

/* ─── COMPONENT: EmptyState ──────────────────── */
const EmptyState = ({ icon, title, sub, action, onAction }) => (
  <div className="empty">
    <div className="empty-icon">{icon}</div>
    <div>
      <p style={{ fontSize:15, fontWeight:500, color:"var(--silver2)", marginBottom:6 }}>{title}</p>
      {sub && <p style={{ fontSize:13, color:"var(--muted)", maxWidth:360, lineHeight:1.6 }}>{sub}</p>}
    </div>
    {action && <button className="btn btn-secondary btn-sm" onClick={onAction}>{action}</button>}
  </div>
);

/* ─────────────────────────────────────────────
   PAGES
───────────────────────────────────────────── */

/* ── HOME PAGE ────────────────────────────────── */
const HomePage = ({ setPage, projects, setSelectedProject }) => {
  return (
    <div className="page content">
      {/* Hero */}
      <section style={{ padding:"60px 0 48px", position:"relative", textAlign:"center" }}>
        <div style={{ position:"absolute", inset:0, pointerEvents:"none" }}>
          <div style={{ position:"absolute", top:"50%", left:"50%", transform:"translate(-50%,-50%)", width:600, height:300, background:"radial-gradient(ellipse,rgba(192,192,192,.04) 0%,transparent 70%)", borderRadius:"50%" }}/>
          <div className="scan-effect"><div className="scan-line"/></div>
        </div>
        <div style={{ position:"relative" }}>
          <div style={{ display:"inline-flex", alignItems:"center", gap:8, padding:"5px 14px", border:"1px solid var(--border2)", borderRadius:20, fontSize:12, color:"var(--muted)", marginBottom:24, background:"var(--bg2)" }}>
            <span className="ndot"/> Platform for Creators
          </div>
          <h1 className="font-serif" style={{ fontSize:"clamp(40px,6vw,72px)", lineHeight:1.1, marginBottom:20, color:"var(--white)" }}>
            Host, explore & share<br/>
            <em style={{ color:"var(--silver)", fontStyle:"italic" }}>complete projects</em>
          </h1>
          <p style={{ fontSize:16, color:"var(--muted)", maxWidth:520, margin:"0 auto 32px", lineHeight:1.8 }}>
            Upload entire project folders. Let the world explore your source code, download your work, and discover your expertise.
          </p>
          <div className="flex justify-center gap-3" style={{ flexWrap:"wrap" }}>
            <button className="btn btn-primary btn-lg" onClick={()=>setPage("upload")}>
              <Icon.Upload/> Upload Project
            </button>
            <button className="btn btn-secondary btn-lg" onClick={()=>setPage("explore")}>
              <Icon.Explore/> Explore
            </button>
          </div>
        </div>
      </section>

      {/* Platform stats */}
      <div className="card" style={{ padding:"24px 32px", marginBottom:40 }}>
        <div style={{ display:"grid", gridTemplateColumns:"repeat(4,1fr)", gap:0 }}>
          {[["0","Projects"],["0","Creators"],["0","Downloads"],["0","Files"]].map(([v,l],i)=>(
            <div key={l} className="stat" style={{ padding:"0 24px", borderLeft:i?`1px solid var(--border)`:"none" }}>
              <div className="stat-val">{v}</div>
              <div className="stat-lbl">{l}</div>
            </div>
          ))}
        </div>
      </div>

      {/* Trending */}
      <section style={{ marginBottom:48 }}>
        <div className="flex items-center justify-between" style={{ marginBottom:20 }}>
          <div className="flex items-center gap-3">
            <Icon.Trending/>
            <h2 style={{ fontSize:18, fontWeight:600, color:"var(--white)" }}>Trending Projects</h2>
          </div>
          <button className="btn btn-ghost btn-sm" onClick={()=>setPage("explore")}>View all →</button>
        </div>
        {projects.length === 0
          ? <EmptyState icon={<Icon.Trending/>} title="No projects yet" sub="Be the first to upload a project and appear here." action="Upload Project" onAction={()=>setPage("upload")}/>
          : <div className="grid-3">{projects.slice(0,6).map(p=><ProjectCard key={p.id} project={p} onClick={(p)=>{setSelectedProject(p);setPage("project");}}/>)}</div>}
      </section>

      {/* Categories */}
      <section style={{ marginBottom:48 }}>
        <h2 style={{ fontSize:18, fontWeight:600, color:"var(--white)", marginBottom:20 }}>Browse Categories</h2>
        <div className="grid-4">
          {CATEGORIES.map(cat=>(
            <div key={cat.id} className="card" style={{ padding:"18px 16px", cursor:"pointer", display:"flex", alignItems:"center", gap:12 }} onClick={()=>setPage("explore")}>
              <div style={{ color:"var(--muted2)", display:"flex" }}>{cat.icon}</div>
              <div>
                <div style={{ fontSize:13.5, fontWeight:500, color:"var(--silver2)" }}>{cat.name}</div>
                <div style={{ fontSize:11, color:"var(--muted2)", marginTop:2 }}>0 projects</div>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Footer */}
      <footer style={{ borderTop:"1px solid var(--border)", paddingTop:32, paddingBottom:16 }}>
        <div className="flex items-center justify-between" style={{ flexWrap:"wrap", gap:12 }}>
          <div className="flex items-center gap-3">
            <div className="sidebar-logo-icon"><Icon.Logo/></div>
            <span className="font-serif" style={{ fontSize:17, color:"var(--white)" }}>Qi <span style={{ color:"var(--silver)" }}>FileHub</span></span>
          </div>
          <div style={{ fontSize:12, color:"var(--muted2)" }}>project hosting for creators</div>
        </div>
      </footer>
    </div>
  );
};

/* ── EXPLORE PAGE ─────────────────────────────── */
const ExplorePage = ({ projects, setPage, setSelectedProject }) => {
  const [search, setSearch] = useState("");
  const [cat, setCat] = useState("all");
  const [sort, setSort] = useState("new");

  const filtered = projects.filter(p => {
    const q = search.toLowerCase();
    const matchQ = !q || p.name?.toLowerCase().includes(q) || p.description?.toLowerCase().includes(q) || (p.tags||[]).some(t=>t.toLowerCase().includes(q)) || p.owner?.toLowerCase().includes(q);
    const matchC = cat === "all" || p.category === cat;
    return matchQ && matchC;
  });

  return (
    <div className="page content">
      <div style={{ marginBottom:24 }}>
        <h1 style={{ fontSize:24, fontWeight:700, color:"var(--white)", marginBottom:6 }}>Explore Projects</h1>
        <p style={{ fontSize:13.5, color:"var(--muted)" }}>Discover work from creators worldwide</p>
      </div>
      {/* Search */}
      <div className="flex gap-3" style={{ marginBottom:20, flexWrap:"wrap" }}>
        <div style={{ flex:1, minWidth:240, position:"relative" }}>
          <span style={{ position:"absolute", left:12, top:"50%", transform:"translateY(-50%)", color:"var(--muted3)", display:"flex" }}><Icon.Search/></span>
          <input className="input" style={{ paddingLeft:40 }} placeholder='Search projects, @username, #tags...' value={search} onChange={e=>setSearch(e.target.value)}/>
        </div>
        <select className="input select" style={{ width:180 }} value={cat} onChange={e=>setCat(e.target.value)}>
          <option value="all">All Categories</option>
          {CATEGORIES.map(c=><option key={c.id} value={c.id}>{c.name}</option>)}
        </select>
        <select className="input select" style={{ width:140 }} value={sort} onChange={e=>setSort(e.target.value)}>
          <option value="new">Newest</option>
          <option value="liked">Most Liked</option>
          <option value="downloaded">Most Downloaded</option>
        </select>
      </div>
      {filtered.length === 0
        ? <EmptyState icon={<Icon.Search/>} title={search?"No results found":"No projects yet"} sub={search?`No projects match "${search}".`:"Projects uploaded by creators will appear here."} action={search?"Clear search":undefined} onAction={()=>setSearch("")}/>
        : <div className="grid-3">
            {filtered.map(p=><ProjectCard key={p.id} project={p} onClick={(p)=>{setSelectedProject(p);setPage("project");}}/>)}
          </div>}
    </div>
  );
};

/* ── PROJECT PAGE ─────────────────────────────── */
const ProjectPage = ({ project, setPage, setMsgTarget }) => {
  const [activeFile, setActiveFile] = useState(null);
  const [liked, setLiked] = useState(false);
  const [saved, setSaved] = useState(false);
  const [comment, setComment] = useState("");
  const [comments, setComments] = useState([]);
  const [tab, setTab] = useState("explorer");

  if (!project) return (
    <div className="page content">
      <EmptyState icon={<Icon.File/>} title="Project not found" sub="This project may have been removed." action="← Back to Explore" onAction={()=>setPage("explore")}/>
    </div>
  );

  return (
    <div className="page">
      <div style={{ display:"flex", gap:0, height:"calc(100vh - 56px)" }}>
        {/* Left: file explorer + code */}
        <div style={{ width:240, minWidth:240, borderRight:"1px solid var(--border)", display:"flex", flexDirection:"column", overflow:"hidden" }}>
          <div style={{ padding:"14px 12px 10px", borderBottom:"1px solid var(--border)" }}>
            <div style={{ fontSize:12, color:"var(--muted)", marginBottom:4, textTransform:"uppercase", letterSpacing:".8px", fontWeight:600 }}>Files</div>
            <div style={{ fontSize:11.5, color:"var(--muted2)" }}>{project.name}</div>
          </div>
          <div style={{ flex:1, overflowY:"auto", padding:"8px 6px" }}>
            <FileTree nodes={DEMO_FILE_TREE} onSelect={setActiveFile} selected={activeFile?.id}/>
          </div>
          <div style={{ padding:"12px 10px", borderTop:"1px solid var(--border)" }}>
            <button className="btn btn-secondary btn-sm w-full" style={{ justifyContent:"center" }} onClick={()=>toast("Download started — ZIP file served by backend")}>
              <Icon.Download/> Download ZIP
            </button>
          </div>
        </div>

        {/* Center: code viewer or project info */}
        <div style={{ flex:1, display:"flex", flexDirection:"column", overflow:"hidden" }}>
          {activeFile
            ? <CodeViewer file={activeFile} onClose={()=>setActiveFile(null)}/>
            : (
              <div style={{ flex:1, overflowY:"auto" }}>
                <div style={{ padding:"28px 28px 0" }}>
                  {project.thumbnail
                    ? <img src={project.thumbnail} alt={project.name} style={{ width:"100%", maxHeight:300, objectFit:"cover", borderRadius:"var(--r2)", border:"1px solid var(--border)", marginBottom:24 }}/>
                    : <div style={{ marginBottom:24 }}><ThumbPlaceholder category={project.category}/></div>}
                  <h1 style={{ fontSize:26, fontWeight:700, color:"var(--white)", marginBottom:10 }}>{project.name}</h1>
                  <div className="flex items-center gap-3" style={{ marginBottom:16, flexWrap:"wrap" }}>
                    <span className="tag tag-silver">{project.category||"General"}</span>
                    {(project.tags||[]).map(t=><span key={t} className="tag">{t}</span>)}
                  </div>
                  <p style={{ fontSize:14, color:"var(--muted)", lineHeight:1.8, marginBottom:20 }}>{project.description||"No description provided."}</p>

                  <div className="tabs" style={{ marginBottom:20 }}>
                    {["Overview","Comments"].map(t=><div key={t} className={`tab ${tab===t.toLowerCase()?"active":""}`} onClick={()=>setTab(t.toLowerCase())}>{t}</div>)}
                  </div>

                  {tab==="overview" && (
                    <div>
                      <div className="grid-2" style={{ marginBottom:20 }}>
                        {project.githubUrl && <a href={project.githubUrl} target="_blank" rel="noreferrer" className="card" style={{ padding:"14px 16px", display:"flex", alignItems:"center", gap:10, textDecoration:"none" }} onClick={e=>e.stopPropagation()}>
                          <Icon.GitHub/><span style={{ fontSize:13, color:"var(--silver2)" }}>GitHub Repo</span><span style={{ marginLeft:"auto", display:"flex", color:"var(--muted2)" }}><Icon.External/></span>
                        </a>}
                        {project.demoUrl && <a href={project.demoUrl} target="_blank" rel="noreferrer" className="card" style={{ padding:"14px 16px", display:"flex", alignItems:"center", gap:10, textDecoration:"none" }} onClick={e=>e.stopPropagation()}>
                          <Icon.External/><span style={{ fontSize:13, color:"var(--silver2)" }}>Live Demo</span><span style={{ marginLeft:"auto", display:"flex", color:"var(--muted2)" }}><Icon.External/></span>
                        </a>}
                      </div>
                    </div>
                  )}
                  {tab==="comments" && (
                    <div>
                      <div className="flex gap-3" style={{ marginBottom:16 }}>
                        <Av name="You" size={32}/>
                        <div style={{ flex:1 }}>
                          <input className="input" placeholder="Leave a comment..." value={comment} onChange={e=>setComment(e.target.value)} onKeyDown={e=>{if(e.key==="Enter"&&comment.trim()){setComments(c=>[...c,{text:comment,t:Date.now()}]);setComment("");toast("Comment posted");}}}/>
                        </div>
                      </div>
                      {comments.length===0
                        ? <EmptyState icon={<Icon.Message/>} title="No comments yet" sub="Be the first to leave a comment."/>
                        : comments.map((c,i)=>(
                          <div key={i} className="flex gap-3" style={{ marginBottom:12 }}>
                            <Av name="You" size={30}/>
                            <div className="card" style={{ flex:1, padding:"10px 14px" }}>
                              <div style={{ fontSize:12, color:"var(--muted2)", marginBottom:4 }}>You · {relTime(c.t)}</div>
                              <p style={{ fontSize:13.5, color:"var(--silver2)", lineHeight:1.6 }}>{c.text}</p>
                            </div>
                          </div>
                        ))}
                    </div>
                  )}
                </div>
              </div>
            )
          }
        </div>

        {/* Right: sidebar info */}
        <div style={{ width:260, minWidth:260, borderLeft:"1px solid var(--border)", padding:"20px 16px", overflowY:"auto", display:"flex", flexDirection:"column", gap:16 }}>
          {/* Owner */}
          <div className="card" style={{ padding:16 }}>
            <div style={{ fontSize:11, color:"var(--muted)", textTransform:"uppercase", letterSpacing:".8px", marginBottom:12, fontWeight:600 }}>Creator</div>
            <div className="flex items-center gap-3" style={{ marginBottom:14 }}>
              <Av name={project.owner} size={40} glow/>
              <div>
                <div style={{ fontSize:14, fontWeight:600, color:"var(--white)" }}>{project.owner||"Unknown"}</div>
                <div style={{ fontSize:12, color:"var(--muted)" }}>@{project.owner||"user"}</div>
              </div>
            </div>
            <button className="btn btn-secondary btn-sm w-full" style={{ justifyContent:"center", marginBottom:8 }} onClick={()=>{setMsgTarget(project.owner);setPage("messages");}}>
              <Icon.Message/> Message Owner
            </button>
            <button className="btn btn-outline btn-sm w-full" style={{ justifyContent:"center" }} onClick={()=>setPage("profile")}>
              View Profile
            </button>
          </div>

          {/* Actions */}
          <div className="flex gap-2">
            <button className="btn btn-secondary flex-1" style={{ justifyContent:"center", gap:6, fontSize:12.5, color:liked?"var(--white)":"var(--muted)" }} onClick={()=>{setLiked(!liked);toast(liked?"Like removed":"Project liked!");}}>
              <Icon.Heart filled={liked}/> {fmtNum((project.likes||0)+(liked?1:0))}
            </button>
            <button className="btn btn-secondary flex-1" style={{ justifyContent:"center", gap:6, fontSize:12.5, color:saved?"var(--white)":"var(--muted)" }} onClick={()=>{setSaved(!saved);toast(saved?"Removed from saved":"Saved!");}}>
              <Icon.Bookmark filled={saved}/>
            </button>
          </div>

          {/* Stats */}
          <div className="card" style={{ padding:16 }}>
            {[["Downloads","0"],["Views","0"],["Likes",fmtNum(project.likes||0)],["Comments",String(comments.length)]].map(([l,v])=>(
              <div key={l} className="flex items-center justify-between" style={{ marginBottom:10, fontSize:13 }}>
                <span style={{ color:"var(--muted)" }}>{l}</span>
                <span style={{ color:"var(--silver2)", fontWeight:500 }}>{v}</span>
              </div>
            ))}
          </div>

          <button className="btn btn-primary" style={{ justifyContent:"center" }} onClick={()=>toast("Download started — ZIP file served by backend")}>
            <Icon.Download/> Download ZIP
          </button>
        </div>
      </div>
    </div>
  );
};

/* ── UPLOAD PAGE ──────────────────────────────── */
const UploadPage = ({ onUpload }) => {
  const [step, setStep] = useState(1);
  const [file, setFile] = useState(null);
  const [drag, setDrag] = useState(false);
  const [form, setForm] = useState({ name:"", description:"", category:"", tags:"", github:"", demo:"" });
  const [thumb, setThumb] = useState(null);
  const [uploading, setUploading] = useState(false);
  const fileRef = useRef();
  const thumbRef = useRef();
  const upd = (k,v) => setForm(p=>({...p,[k]:v}));

  const handleDrop = (e) => {
    e.preventDefault(); setDrag(false);
    const f = e.dataTransfer.files[0];
    if(f?.name.endsWith(".zip")) setFile(f);
    else toast("Please upload a .zip file");
  };

  const handleSubmit = () => {
    if(!form.name||!form.category) { toast("Fill in required fields"); return; }
    if(!file) { toast("Please upload a ZIP file"); return; }
    setUploading(true);
    setTimeout(()=>{
      onUpload({ id:Date.now(), name:form.name, description:form.description, category:form.category, tags:form.tags.split(",").map(t=>t.trim()).filter(Boolean), githubUrl:form.github, demoUrl:form.demo, thumbnail:thumb, owner:"you", likes:0, createdAt:Date.now() });
      setUploading(false);
      toast("🎉 Project uploaded successfully!");
      setStep(1); setFile(null); setForm({name:"",description:"",category:"",tags:"",github:"",demo:""}); setThumb(null);
    }, 1600);
  };

  return (
    <div className="page content" style={{ maxWidth:680 }}>
      <div style={{ marginBottom:28 }}>
        <h1 style={{ fontSize:24, fontWeight:700, color:"var(--white)", marginBottom:6 }}>Upload Project</h1>
        <p style={{ fontSize:13.5, color:"var(--muted)" }}>Share your work with the creator community</p>
      </div>

      {/* Steps */}
      <div className="flex items-center gap-0" style={{ marginBottom:32 }}>
        {["Project Files","Details","Review"].map((label,i)=>{
          const s = i+1;
          return (
            <div key={s} className="flex items-center" style={{ flex:s<3?1:"initial" }}>
              <div className="flex items-center gap-2" style={{ cursor:"pointer" }} onClick={()=>s<step&&setStep(s)}>
                <div style={{ width:28, height:28, borderRadius:"50%", display:"flex", alignItems:"center", justifyContent:"center", fontWeight:700, fontSize:12, fontFamily:"'Geist',sans-serif", flexShrink:0, background:step>s?"var(--silver)":step===s?"var(--white)":"var(--bg4)", color:step>=s?"#050505":"var(--muted3)", border:`1.5px solid ${step>=s?"transparent":"var(--border)"}`, transition:"all .25s" }}>{step>s?"✓":s}</div>
                <span style={{ fontSize:12.5, color:step>=s?"var(--silver2)":"var(--muted3)", whiteSpace:"nowrap" }}>{label}</span>
              </div>
              {s<3 && <div style={{ flex:1, height:1, background:step>s?"var(--border2)":"var(--border)", margin:"0 12px", transition:"background .3s" }}/>}
            </div>
          );
        })}
      </div>

      <div className="card-glass" style={{ padding:28 }}>
        {step===1 && (
          <div>
            <h3 style={{ fontSize:15, fontWeight:600, color:"var(--white)", marginBottom:16 }}>Upload ZIP Archive</h3>
            <div className={`dropzone ${drag?"drag":""}`} onDragOver={e=>{e.preventDefault();setDrag(true);}} onDragLeave={()=>setDrag(false)} onDrop={handleDrop} onClick={()=>fileRef.current?.click()}>
              <input ref={fileRef} type="file" accept=".zip" onChange={e=>{const f=e.target.files[0];if(f)setFile(f);}} hidden/>
              {file
                ? <div className="flex flex-col items-center gap-3">
                    <div style={{ fontSize:36 }}>📦</div>
                    <div style={{ fontSize:14, fontWeight:500, color:"var(--silver2)" }}>{file.name}</div>
                    <div style={{ fontSize:12, color:"var(--muted)" }}>{(file.size/1024/1024).toFixed(2)} MB</div>
                    <button className="btn btn-secondary btn-sm" onClick={e=>{e.stopPropagation();setFile(null);}}>Remove</button>
                  </div>
                : <div className="flex flex-col items-center gap-3">
                    <div style={{ width:48, height:48, borderRadius:12, border:"1px solid var(--border2)", display:"flex", alignItems:"center", justifyContent:"center", color:"var(--muted)" }}><Icon.Upload/></div>
                    <div>
                      <p style={{ fontSize:14, fontWeight:500, color:"var(--silver2)", marginBottom:4 }}>Drop your ZIP archive here</p>
                      <p style={{ fontSize:12.5, color:"var(--muted)" }}>or click to browse — ZIP files only</p>
                    </div>
                  </div>}
            </div>
            <p style={{ fontSize:11.5, color:"var(--muted2)", marginTop:12 }}>⟡ Your project folder will be extracted and stored. All file paths and structure are preserved.</p>
          </div>
        )}

        {step===2 && (
          <div style={{ display:"flex", flexDirection:"column", gap:18 }}>
            <h3 style={{ fontSize:15, fontWeight:600, color:"var(--white)", marginBottom:4 }}>Project Details</h3>

            {/* Thumbnail */}
            <div>
              <label className="input-label">Project Thumbnail <span style={{ color:"#ef4444" }}>*</span></label>
              <div style={{ display:"flex", gap:12, alignItems:"flex-start" }}>
                <div style={{ width:140, aspectRatio:"16/9", borderRadius:"var(--r)", overflow:"hidden", border:"1px solid var(--border)", cursor:"pointer", flexShrink:0 }} onClick={()=>thumbRef.current?.click()}>
                  {thumb ? <img src={thumb} alt="thumb" style={{ width:"100%", height:"100%", objectFit:"cover" }}/> : <div style={{ width:"100%", height:"100%", background:"var(--bg3)", display:"flex", alignItems:"center", justifyContent:"center", flexDirection:"column", gap:6 }}><span style={{ color:"var(--muted3)", display:"flex" }}><Icon.Upload/></span><span style={{ fontSize:10, color:"var(--muted3)" }}>Upload</span></div>}
                </div>
                <input ref={thumbRef} type="file" accept="image/*" hidden onChange={e=>{const f=e.target.files[0];if(f)setThumb(URL.createObjectURL(f));}}/>
                <div>
                  <p style={{ fontSize:12, color:"var(--muted)", lineHeight:1.6 }}>Upload a 16:9 thumbnail. This appears on the homepage, search results and your profile.</p>
                  <button className="btn btn-secondary btn-sm" style={{ marginTop:8 }} onClick={()=>thumbRef.current?.click()}>Choose Image</button>
                </div>
              </div>
            </div>

            {[
              { k:"name", l:"Project Name", ph:"e.g., My Awesome Web App", req:true },
              { k:"description", l:"Description", ph:"What does your project do? What problem does it solve?", type:"textarea" },
            ].map(f=>(
              <div key={f.k}>
                <label className="input-label">{f.l} {f.req&&<span style={{ color:"#ef4444" }}>*</span>}</label>
                {f.type==="textarea"
                  ? <textarea className="input textarea" placeholder={f.ph} value={form[f.k]} onChange={e=>upd(f.k,e.target.value)}/>
                  : <input className="input" placeholder={f.ph} value={form[f.k]} onChange={e=>upd(f.k,e.target.value)}/>}
              </div>
            ))}

            <div>
              <label className="input-label">Category <span style={{ color:"#ef4444" }}>*</span></label>
              <select className="input select" value={form.category} onChange={e=>upd("category",e.target.value)}>
                <option value="">Select a category</option>
                {CATEGORIES.map(c=><option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>

            <div>
              <label className="input-label">Tags <span style={{ color:"var(--muted2)", fontWeight:400 }}>(comma separated)</span></label>
              <input className="input" placeholder="react, python, ai, open-source..." value={form.tags} onChange={e=>upd("tags",e.target.value)}/>
            </div>

            <div className="grid-2">
              <div>
                <label className="input-label">GitHub URL</label>
                <input className="input" placeholder="https://github.com/..." value={form.github} onChange={e=>upd("github",e.target.value)}/>
              </div>
              <div>
                <label className="input-label">Live Demo URL</label>
                <input className="input" placeholder="https://..." value={form.demo} onChange={e=>upd("demo",e.target.value)}/>
              </div>
            </div>
          </div>
        )}

        {step===3 && (
          <div>
            <h3 style={{ fontSize:15, fontWeight:600, color:"var(--white)", marginBottom:20 }}>Review & Publish</h3>
            <div className="card-inset" style={{ padding:18, marginBottom:16 }}>
              {thumb && <img src={thumb} alt="thumb" style={{ width:"100%", aspectRatio:"16/9", objectFit:"cover", borderRadius:"var(--r)", marginBottom:14, border:"1px solid var(--border)" }}/>}
              <h4 style={{ fontSize:16, fontWeight:600, color:"var(--white)", marginBottom:6 }}>{form.name||"Untitled"}</h4>
              <p style={{ fontSize:13, color:"var(--muted)", lineHeight:1.6, marginBottom:12 }}>{form.description||"—"}</p>
              <div className="flex gap-2" style={{ flexWrap:"wrap", marginBottom:10 }}>
                {form.category&&<span className="tag tag-silver">{form.category}</span>}
                {form.tags.split(",").filter(Boolean).map(t=><span key={t} className="tag">{t.trim()}</span>)}
              </div>
              <div className="shine-line" style={{ marginBottom:12 }}/>
              <div style={{ fontSize:12, color:"var(--muted)" }}>📦 {file?.name||"No file"} · {file?`${(file.size/1024/1024).toFixed(2)} MB`:""}</div>
            </div>
            <div className="card-inset" style={{ padding:14, fontSize:13, color:"var(--muted)", lineHeight:1.7 }}>
              ⟡ Your ZIP will be extracted on the server. The full file tree will be available for browsing. All files remain downloadable.
            </div>
          </div>
        )}

        {/* Nav */}
        <div className="flex justify-between" style={{ marginTop:24 }}>
          {step>1
            ? <button className="btn btn-secondary" onClick={()=>setStep(s=>s-1)}><Icon.ChevLeft/> Back</button>
            : <div/>}
          {step<3
            ? <button className="btn btn-primary" onClick={()=>{if(step===1&&!file){toast("Upload a ZIP file first");return;}setStep(s=>s+1);}}>Continue →</button>
            : <button className="btn btn-primary" disabled={uploading} onClick={handleSubmit}>
                {uploading ? "Publishing…" : "Publish Project"}
              </button>}
        </div>
      </div>
    </div>
  );
};

/* ── PROFILE PAGE ─────────────────────────────── */
const ProfilePage = ({ projects, setPage, setSelectedProject }) => {
  const [tab, setTab] = useState("Projects");
  const [following, setFollowing] = useState(false);
  const tabs = ["Projects","About","Activity"];

  return (
    <div className="page">
      <div className="profile-cover">
        <div className="cover-pattern"/>
        <div style={{ position:"absolute", bottom:16, right:20, display:"flex", gap:8 }}>
          <button className="btn btn-secondary btn-sm" onClick={()=>setPage("settings")}><Icon.Settings/> Edit Profile</button>
        </div>
      </div>

      <div style={{ padding:"0 28px" }}>
        <div className="flex items-end justify-between" style={{ marginBottom:20, marginTop:-24 }}>
          <div className="flex items-end gap-4">
            <Av name="You" size={72} glow/>
            <div style={{ paddingBottom:6 }}>
              <div style={{ fontSize:20, fontWeight:700, color:"var(--white)" }}>Your Name</div>
              <div style={{ fontSize:13.5, color:"var(--muted)" }}>@username</div>
            </div>
          </div>
          <div className="flex gap-2" style={{ paddingBottom:8 }}>
            <button className={`btn btn-sm ${following?"btn-secondary":"btn-primary"}`} onClick={()=>setFollowing(!following)}>
              {following?"Following":"Follow"}
            </button>
            <button className="btn btn-secondary btn-sm" onClick={()=>setPage("messages")}><Icon.Message/> Message</button>
          </div>
        </div>

        <div className="flex gap-8" style={{ marginBottom:20 }}>
          {[["0","Projects"],["0","Followers"],["0","Following"]].map(([v,l])=>(
            <div key={l} className="stat" style={{ textAlign:"left" }}>
              <div className="stat-val" style={{ fontSize:18 }}>{v}</div>
              <div className="stat-lbl">{l}</div>
            </div>
          ))}
        </div>

        <div className="tabs" style={{ marginBottom:24 }}>
          {tabs.map(t=><div key={t} className={`tab ${tab===t?"active":""}`} onClick={()=>setTab(t)}>{t}</div>)}
        </div>

        {tab==="Projects" && (
          projects.length===0
            ? <EmptyState icon={<Icon.Upload/>} title="No projects yet" sub="Upload your first project to showcase your work." action="Upload Project" onAction={()=>setPage("upload")}/>
            : <div className="grid-3" style={{ paddingBottom:32 }}>
                {projects.map(p=><ProjectCard key={p.id} project={p} onClick={(p)=>{setSelectedProject(p);setPage("project");}}/>)}
              </div>
        )}
        {tab==="About" && (
          <div style={{ maxWidth:540, paddingBottom:32 }}>
            <div className="card-inset" style={{ padding:20 }}>
              <p style={{ fontSize:13.5, color:"var(--muted)", lineHeight:1.8 }}>No bio added yet. Edit your profile to add a bio, skills, and social links.</p>
            </div>
          </div>
        )}
        {tab==="Activity" && (
          <EmptyState icon={<Icon.Zap/>} title="No activity yet" sub="Activity from likes, comments, and follows will appear here."/>
        )}
      </div>
    </div>
  );
};

/* ── MESSAGES PAGE ────────────────────────────── */
const MessagesPage = ({ msgTarget, setMsgTarget }) => {
  const [conversations, setConversations] = useState([]);
  const [active, setActive] = useState(null);
  const [input, setInput] = useState("");
  const [msgs, setMsgs] = useState({});
  const endRef = useRef();

  useEffect(()=>{
    if(msgTarget && !conversations.find(c=>c.id===msgTarget)){
      setConversations(prev=>[{id:msgTarget,name:msgTarget,lastMsg:"",t:Date.now()},...prev]);
      setActive(msgTarget);
    }
    if(msgTarget) { setActive(msgTarget); setMsgTarget(null); }
  },[msgTarget]);

  useEffect(()=>endRef.current?.scrollIntoView({behavior:"smooth"}),[msgs,active]);

  const send = () => {
    if(!input.trim()||!active) return;
    const m = {text:input,from:"me",t:Date.now()};
    setMsgs(p=>({...p,[active]:[...(p[active]||[]),m]}));
    setConversations(p=>p.map(c=>c.id===active?{...c,lastMsg:input,t:Date.now()}:c));
    setInput(""); toast("Message sent");
  };

  return (
    <div className="page" style={{ display:"flex", height:"calc(100vh - 56px)", overflow:"hidden" }}>
      {/* Inbox */}
      <div style={{ width:260, borderRight:"1px solid var(--border)", display:"flex", flexDirection:"column" }}>
        <div style={{ padding:"16px 14px", borderBottom:"1px solid var(--border)" }}>
          <h2 style={{ fontSize:15, fontWeight:600, color:"var(--white)", marginBottom:10 }}>Messages</h2>
          <input className="input" style={{ fontSize:12.5 }} placeholder="Search conversations..."/>
        </div>
        <div style={{ flex:1, overflowY:"auto" }}>
          {conversations.length===0
            ? <div className="empty" style={{ padding:"40px 16px" }}>
                <Icon.Message/>
                <p style={{ fontSize:12.5, color:"var(--muted)" }}>No conversations yet</p>
              </div>
            : conversations.map(c=>(
              <div key={c.id} style={{ padding:"12px 14px", cursor:"pointer", display:"flex", alignItems:"center", gap:10, background:active===c.id?"var(--glow)":"transparent", borderBottom:"1px solid var(--border)", transition:"background .15s" }} onClick={()=>setActive(c.id)}>
                <Av name={c.name} size={36}/>
                <div style={{ flex:1, minWidth:0 }}>
                  <div style={{ fontSize:13.5, fontWeight:500, color:active===c.id?"var(--white)":"var(--silver2)" }}>@{c.name}</div>
                  <div style={{ fontSize:11.5, color:"var(--muted)", overflow:"hidden", textOverflow:"ellipsis", whiteSpace:"nowrap" }}>{c.lastMsg||"No messages yet"}</div>
                </div>
              </div>
            ))}
        </div>
        <div style={{ padding:12, borderTop:"1px solid var(--border)" }}>
          <button className="btn btn-secondary btn-sm w-full" style={{ justifyContent:"center" }} onClick={()=>{const n=prompt("Message @username:");if(n){const id=n.replace("@","");setConversations(p=>[{id,name:id,lastMsg:"",t:Date.now()},...p.filter(c=>c.id!==id)]);setActive(id);}}}>
            <Icon.Plus/> New Message
          </button>
        </div>
      </div>

      {/* Chat */}
      {active
        ? (
          <div style={{ flex:1, display:"flex", flexDirection:"column" }}>
            <div className="flex items-center gap-3" style={{ padding:"12px 20px", borderBottom:"1px solid var(--border)", background:"var(--bg1)" }}>
              <Av name={active} size={34}/>
              <div>
                <div style={{ fontSize:14, fontWeight:600, color:"var(--white)" }}>@{active}</div>
              </div>
            </div>
            <div style={{ flex:1, overflowY:"auto", padding:"20px 20px 16px", display:"flex", flexDirection:"column", gap:10 }}>
              {(msgs[active]||[]).map((m,i)=>(
                <div key={i} style={{ display:"flex", justifyContent:m.from==="me"?"flex-end":"flex-start", gap:8 }}>
                  {m.from!=="me"&&<Av name={active} size={28}/>}
                  <div className={`bubble ${m.from==="me"?"bubble-me":"bubble-them"}`}>{m.text}</div>
                </div>
              ))}
              {(msgs[active]||[]).length===0&&<div style={{ textAlign:"center", color:"var(--muted)", fontSize:13, marginTop:60 }}>Send a message to @{active}</div>}
              <div ref={endRef}/>
            </div>
            <div className="flex gap-2" style={{ padding:"12px 16px", borderTop:"1px solid var(--border)", background:"var(--bg1)" }}>
              <input className="input" style={{ flex:1 }} placeholder="Type a message..." value={input} onChange={e=>setInput(e.target.value)} onKeyDown={e=>e.key==="Enter"&&send()}/>
              <button className="btn btn-primary" onClick={send}><Icon.Send/></button>
            </div>
          </div>
        )
        : (
          <div className="flex-1 flex flex-col items-center justify-center" style={{ color:"var(--muted)", textAlign:"center", gap:12 }}>
            <div style={{ width:52, height:52, borderRadius:16, border:"1px solid var(--border)", display:"flex", alignItems:"center", justifyContent:"center" }}><Icon.Message/></div>
            <p style={{ fontSize:13.5 }}>Select a conversation</p>
          </div>
        )}
    </div>
  );
};

/* ── NOTIFICATIONS PAGE ───────────────────────── */
const NotificationsPage = () => {
  const [notifs, setNotifs] = useState([]);
  return (
    <div className="page content" style={{ maxWidth:640 }}>
      <div className="flex items-center justify-between" style={{ marginBottom:24 }}>
        <h1 style={{ fontSize:22, fontWeight:700, color:"var(--white)" }}>Notifications</h1>
        {notifs.length>0&&<button className="btn btn-ghost btn-sm" onClick={()=>setNotifs([])}>Clear all</button>}
      </div>
      {notifs.length===0
        ? <EmptyState icon={<Icon.Bell/>} title="No notifications" sub="You'll see notifications for new followers, likes, comments, and messages here."/>
        : notifs.map((n,i)=>(
          <div key={i} className="card" style={{ padding:"14px 16px", marginBottom:8, display:"flex", alignItems:"center", gap:12 }}>
            <Av name={n.user} size={34}/>
            <div style={{ flex:1 }}>
              <span style={{ fontSize:13.5, color:"var(--silver2)" }}><strong style={{ color:"var(--white)" }}>@{n.user}</strong> {n.msg}</span>
              <div style={{ fontSize:11.5, color:"var(--muted2)", marginTop:3 }}>{relTime(n.t)}</div>
            </div>
          </div>
        ))}
    </div>
  );
};

/* ── SETTINGS PAGE ────────────────────────────── */
const SettingsPage = () => {
  const [tab, setTab] = useState("Profile");
  const tabs = ["Profile","Account","Appearance","Danger Zone"];
  return (
    <div className="page content">
      <h1 style={{ fontSize:22, fontWeight:700, color:"var(--white)", marginBottom:24 }}>Settings</h1>
      <div className="flex gap-6" style={{ flexWrap:"wrap" }}>
        <div style={{ width:180, flexShrink:0 }}>
          {tabs.map(t=>(
            <button key={t} style={{ display:"block", width:"100%", textAlign:"left", padding:"9px 12px", borderRadius:"var(--r)", fontSize:13.5, fontFamily:"'Geist',sans-serif", cursor:"pointer", background:tab===t?"var(--glowHover)":"transparent", color:tab===t?"var(--white)":"var(--muted)", border:"none", marginBottom:2, transition:"all .15s" }} onClick={()=>setTab(t)}>{t}</button>
          ))}
        </div>
        <div className="card-glass" style={{ flex:1, padding:28, minWidth:300 }}>
          {tab==="Profile"&&(
            <div style={{ display:"flex", flexDirection:"column", gap:18 }}>
              <h3 style={{ fontSize:14.5, fontWeight:600, color:"var(--white)", marginBottom:4 }}>Profile Information</h3>
              <div className="flex items-center gap-4">
                <Av name="You" size={60} glow/>
                <div><button className="btn btn-secondary btn-sm">Change Photo</button><p style={{ fontSize:11.5, color:"var(--muted)", marginTop:6 }}>JPG, PNG up to 5MB</p></div>
              </div>
              {[["Display Name","Your Name"],["Username","username"],["Bio","Tell us about yourself...","textarea"],["Website","https://yoursite.com"],["Skills","React, Python, Figma, ..."]].map(([l,ph,type])=>(
                <div key={l}>
                  <label className="input-label">{l}</label>
                  {type==="textarea"?<textarea className="input textarea" placeholder={ph} style={{ minHeight:70 }}/>:<input className="input" placeholder={ph}/>}
                </div>
              ))}
              <button className="btn btn-primary" style={{ alignSelf:"flex-start" }}>Save Changes</button>
            </div>
          )}
          {tab==="Account"&&(
            <div style={{ display:"flex", flexDirection:"column", gap:18 }}>
              <h3 style={{ fontSize:14.5, fontWeight:600, color:"var(--white)", marginBottom:4 }}>Account Security</h3>
              {[["Email","your@email.com","email"],["Current Password","","password"],["New Password","","password"]].map(([l,ph,t])=>(
                <div key={l}><label className="input-label">{l}</label><input className="input" type={t||"text"} placeholder={ph}/></div>
              ))}
              <div className="card-inset" style={{ padding:16 }}>
                <p style={{ fontSize:13, color:"var(--muted)", marginBottom:12 }}>Connected Accounts</p>
                {[["GitHub","Not connected"],["Google","Not connected"]].map(([name,status])=>(
                  <div key={name} className="flex items-center justify-between" style={{ marginBottom:8 }}>
                    <span style={{ fontSize:13.5, color:"var(--silver2)" }}>{name}</span>
                    <button className="btn btn-secondary btn-sm">{status}</button>
                  </div>
                ))}
              </div>
              <button className="btn btn-primary" style={{ alignSelf:"flex-start" }}>Update Account</button>
            </div>
          )}
          {tab==="Appearance"&&(
            <div>
              <h3 style={{ fontSize:14.5, fontWeight:600, color:"var(--white)", marginBottom:16 }}>Appearance</h3>
              <div style={{ display:"grid", gridTemplateColumns:"1fr 1fr", gap:12 }}>
                {["Dark (Default)","High Contrast"].map((t,i)=>(
                  <div key={t} className="card-inset" style={{ padding:16, cursor:"pointer", border:i===0?"1px solid var(--border2)":"1px solid var(--border)" }}>
                    <div style={{ height:60, borderRadius:6, background:i===0?"#050505":"#000", border:"1px solid var(--border2)", marginBottom:10 }}/>
                    <p style={{ fontSize:13, color:i===0?"var(--white)":"var(--muted)", fontWeight:i===0?500:400 }}>{t}</p>
                    {i===0&&<span style={{ fontSize:11, color:"var(--muted2)" }}>Current</span>}
                  </div>
                ))}
              </div>
            </div>
          )}
          {tab==="Danger Zone"&&(
            <div>
              <h3 style={{ fontSize:14.5, fontWeight:600, color:"#ef4444", marginBottom:16 }}>Danger Zone</h3>
              <div className="card-inset" style={{ padding:20, borderColor:"rgba(239,68,68,.2)" }}>
                <p style={{ fontSize:13.5, color:"var(--silver2)", marginBottom:6, fontWeight:500 }}>Delete Account</p>
                <p style={{ fontSize:12.5, color:"var(--muted)", marginBottom:14, lineHeight:1.6 }}>Permanently delete your account and all associated data. This action cannot be undone.</p>
                <button className="btn btn-danger btn-sm" onClick={()=>toast("Please contact support to delete your account")}>Delete My Account</button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

/* ── ADMIN PAGE ───────────────────────────────── */
const AdminPage = ({ projects, onRemove }) => {
  const [tab, setTab] = useState("Overview");
  const tabs = ["Overview","Projects","Users","Reports"];
  return (
    <div className="page content">
      <div className="flex items-center gap-3" style={{ marginBottom:24 }}>
        <Icon.Admin/>
        <div>
          <h1 style={{ fontSize:22, fontWeight:700, color:"var(--white)" }}>Admin Panel</h1>
          <p style={{ fontSize:12.5, color:"var(--muted)" }}>Platform management & moderation</p>
        </div>
      </div>
      <div className="tabs" style={{ marginBottom:24 }}>
        {tabs.map(t=><div key={t} className={`tab ${tab===t?"active":""}`} onClick={()=>setTab(t)}>{t}</div>)}
      </div>
      {tab==="Overview"&&(
        <div>
          <div className="grid-4" style={{ marginBottom:24 }}>
            {[["Total Users","0",<Icon.Profile/>],["Projects",""+projects.length,<Icon.Grid/>],["Downloads","0",<Icon.Download/>],["Reports","0",<Icon.Bell/>]].map(([l,v,i])=>(
              <div key={l} className="card" style={{ padding:"20px 18px" }}>
                <div style={{ color:"var(--muted2)", display:"flex", marginBottom:12 }}>{i}</div>
                <div style={{ fontSize:26, fontWeight:700, fontFamily:"'Instrument Serif',serif", color:"var(--white)", marginBottom:4 }}>{v}</div>
                <div style={{ fontSize:12, color:"var(--muted)" }}>{l}</div>
              </div>
            ))}
          </div>
          <div className="card-inset" style={{ padding:20 }}>
            <p style={{ fontSize:13, color:"var(--muted)" }}>Backend analytics — connect FastAPI backend to see real-time platform data.</p>
          </div>
        </div>
      )}
      {tab==="Projects"&&(
        <div>
          {projects.length===0
            ? <EmptyState icon={<Icon.Grid/>} title="No projects" sub="Uploaded projects will appear here."/>
            : projects.map(p=>(
              <div key={p.id} className="card" style={{ padding:"14px 18px", marginBottom:8, display:"flex", alignItems:"center", gap:14 }}>
                <div style={{ width:48, aspectRatio:"16/9", borderRadius:6, overflow:"hidden", border:"1px solid var(--border)", flexShrink:0 }}>
                  {p.thumbnail?<img src={p.thumbnail} style={{ width:"100%", height:"100%", objectFit:"cover" }}/>:<ThumbPlaceholder category={p.category}/>}
                </div>
                <div style={{ flex:1, minWidth:0 }}>
                  <div style={{ fontSize:13.5, fontWeight:500, color:"var(--white)", marginBottom:2 }}>{p.name}</div>
                  <div style={{ fontSize:12, color:"var(--muted)" }}>by @{p.owner} · {relTime(p.createdAt||Date.now())}</div>
                </div>
                <button className="btn btn-danger btn-sm" onClick={()=>{onRemove(p.id);toast("Project removed");}}>Remove</button>
              </div>
            ))}
        </div>
      )}
      {tab==="Users"&&<EmptyState icon={<Icon.Profile/>} title="No users" sub="Registered users will appear here."/>}
      {tab==="Reports"&&<EmptyState icon={<Icon.Bell/>} title="No reports" sub="Content reports from users will appear here."/>}
    </div>
  );
};

/* ── AUTH MODAL ───────────────────────────────── */
const AuthModal = ({ mode="login", onClose }) => {
  const [m, setM] = useState(mode);
  const [form, setForm] = useState({ username:"", email:"", password:"", confirm:"" });
  const upd = (k,v) => setForm(p=>({...p,[k]:v}));

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e=>e.stopPropagation()}>
        <div className="flex items-center justify-between" style={{ marginBottom:24 }}>
          <div className="flex items-center gap-3">
            <div className="sidebar-logo-icon"><Icon.Logo/></div>
            <span className="font-serif" style={{ fontSize:18, color:"var(--white)" }}>Qi <span style={{ color:"var(--silver)" }}>FileHub</span></span>
          </div>
          <button className="btn btn-ghost btn-sm" style={{ padding:6 }} onClick={onClose}><Icon.X/></button>
        </div>
        <h2 style={{ fontSize:19, fontWeight:600, color:"var(--white)", marginBottom:6 }}>{m==="login"?"Welcome back":"Create account"}</h2>
        <p style={{ fontSize:13, color:"var(--muted)", marginBottom:22 }}>{m==="login"?"Sign in to your account":"Join the creator community"}</p>

        {/* OAuth */}
        <div style={{ display:"flex", flexDirection:"column", gap:10, marginBottom:20 }}>
          {[["GitHub","🐙"],["Google","🔵"]].map(([name,ico])=>(
            <button key={name} className="btn btn-secondary w-full" style={{ justifyContent:"center", gap:8 }} onClick={()=>{toast(`${name} OAuth — connect your backend`);onClose();}}>
              {ico} Continue with {name}
            </button>
          ))}
        </div>
        <div className="flex items-center gap-3" style={{ marginBottom:20 }}>
          <div className="divider" style={{ flex:1 }}/><span style={{ fontSize:11.5, color:"var(--muted2)", whiteSpace:"nowrap" }}>or with email</span><div className="divider" style={{ flex:1 }}/>
        </div>

        <div style={{ display:"flex", flexDirection:"column", gap:14 }}>
          {m==="register"&&<div><label className="input-label">Username</label><input className="input" placeholder="@username" value={form.username} onChange={e=>upd("username",e.target.value)}/></div>}
          <div><label className="input-label">Email</label><input className="input" type="email" placeholder="you@example.com" value={form.email} onChange={e=>upd("email",e.target.value)}/></div>
          <div><label className="input-label">Password</label><input className="input" type="password" placeholder="••••••••" value={form.password} onChange={e=>upd("password",e.target.value)}/></div>
          {m==="register"&&<div><label className="input-label">Confirm Password</label><input className="input" type="password" placeholder="••••••••" value={form.confirm} onChange={e=>upd("confirm",e.target.value)}/></div>}
          <button className="btn btn-primary w-full" style={{ justifyContent:"center", padding:12 }} onClick={()=>{toast(m==="login"?"Signed in — connect your backend":"Account created — connect your backend");onClose();}}>
            {m==="login"?"Sign In →":"Create Account →"}
          </button>
        </div>

        <div style={{ textAlign:"center", marginTop:16, fontSize:13, color:"var(--muted)" }}>
          {m==="login"?"Don't have an account? ":"Already have an account? "}
          <span style={{ color:"var(--silver2)", cursor:"pointer", textDecoration:"underline" }} onClick={()=>setM(m==="login"?"register":"login")}>
            {m==="login"?"Sign up":"Sign in"}
          </span>
        </div>
      </div>
    </div>
  );
};

/* ─────────────────────────────────────────────
   ROOT APP
───────────────────────────────────────────── */
export default function App() {
  const [page, setPage] = useState("home");
  const [collapsed, setCollapsed] = useState(false);
  const [projects, setProjects] = useState([]);
  const [selectedProject, setSelectedProject] = useState(null);
  const [authModal, setAuthModal] = useState(null);
  const [msgTarget, setMsgTarget] = useState(null);

  const navGroups = [
    { label:"Discover", items:[
      { id:"home", label:"Home", icon:<Icon.Home/> },
      { id:"explore", label:"Explore", icon:<Icon.Explore/> },
      { id:"trending", label:"Trending", icon:<Icon.Trending/> },
    ]},
    { label:"Create", items:[
      { id:"upload", label:"Upload", icon:<Icon.Upload/> },
    ]},
    { label:"You", items:[
      { id:"profile", label:"Profile", icon:<Icon.Profile/> },
      { id:"notifications", label:"Notifications", icon:<Icon.Bell/>, badge:0 },
      { id:"messages", label:"Messages", icon:<Icon.Message/>, badge:0 },
      { id:"settings", label:"Settings", icon:<Icon.Settings/> },
    ]},
    { label:"Platform", items:[
      { id:"admin", label:"Admin", icon:<Icon.Admin/> },
    ]},
  ];

  const renderPage = () => {
    switch(page){
      case "home": return <HomePage setPage={setPage} projects={projects} setSelectedProject={setSelectedProject}/>;
      case "explore": return <ExplorePage projects={projects} setPage={setPage} setSelectedProject={setSelectedProject}/>;
      case "trending": return <ExplorePage projects={[...projects].sort((a,b)=>(b.likes||0)-(a.likes||0))} setPage={setPage} setSelectedProject={setSelectedProject}/>;
      case "upload": return <UploadPage onUpload={p=>{setProjects(prev=>[p,...prev]);setTimeout(()=>setPage("profile"),200);}}/>;
      case "project": return <ProjectPage project={selectedProject} setPage={setPage} setMsgTarget={setMsgTarget}/>;
      case "profile": return <ProfilePage projects={projects} setPage={setPage} setSelectedProject={setSelectedProject}/>;
      case "messages": return <MessagesPage msgTarget={msgTarget} setMsgTarget={setMsgTarget}/>;
      case "notifications": return <NotificationsPage/>;
      case "settings": return <SettingsPage/>;
      case "admin": return <AdminPage projects={projects} onRemove={id=>setProjects(p=>p.filter(x=>x.id!==id))}/>;
      default: return <HomePage setPage={setPage} projects={projects} setSelectedProject={setSelectedProject}/>;
    }
  };

  return (
    <>
      <style>{CSS}</style>
      <div className="app">
        {/* Sidebar */}
        <div className={`sidebar ${collapsed?"collapsed":""}`}>
          <div className="sidebar-logo">
            <div className="sidebar-logo-icon"><Icon.Logo/></div>
            <span className="sidebar-logo-text">Qi <span>FileHub</span></span>
            <div style={{ marginLeft:"auto", flexShrink:0 }}>
              <div className="hamburger" onClick={()=>setCollapsed(!collapsed)}><Icon.Menu/></div>
            </div>
          </div>

          {navGroups.map(group=>(
            <div key={group.label}>
              <div className="sidebar-section-label">{group.label}</div>
              {group.items.map(item=>(
                <div key={item.id} className={`sl ${page===item.id?"active":""}`} onClick={()=>setPage(item.id)}
                  title={collapsed?item.label:undefined}>
                  <span className="sl-icon">{item.icon}</span>
                  <span className="sl-label">{item.label}</span>
                  {item.badge>0&&<span className="sl-badge">{item.badge}</span>}
                </div>
              ))}
            </div>
          ))}

          <div className="sidebar-user" onClick={()=>setAuthModal("login")}>
            <div className="sidebar-user-av">Y</div>
            <div className="sidebar-user-info">
              <div className="sidebar-user-name">@username</div>
              <div className="sidebar-user-handle">Sign in / Register</div>
            </div>
          </div>
        </div>

        {/* Main */}
        <div className="main-area">
          {/* Topbar */}
          <div className="topbar">
            <div className="flex items-center gap-3" style={{ flex:1 }}>
              <div style={{ position:"relative", width:320 }}>
                <span style={{ position:"absolute", left:11, top:"50%", transform:"translateY(-50%)", color:"var(--muted3)", display:"flex", pointerEvents:"none" }}><Icon.Search/></span>
                <input className="input" style={{ paddingLeft:36, height:36, fontSize:13 }} placeholder="Search projects, @users, #tags..."/>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <button className="btn btn-ghost btn-sm" style={{ position:"relative" }} onClick={()=>setPage("notifications")}>
                <Icon.Bell/>
              </button>
              <button className="btn btn-ghost btn-sm" onClick={()=>setPage("messages")}><Icon.Message/></button>
              <div style={{ width:1, height:20, background:"var(--border)", margin:"0 4px" }}/>
              <button className="btn btn-primary btn-sm" onClick={()=>setPage("upload")}><Icon.Upload/> Upload</button>
              <button className="btn btn-secondary btn-sm" onClick={()=>setAuthModal("login")}>Sign In</button>
            </div>
          </div>

          {/* Content */}
          <div style={{ flex:1, overflowY:["project","messages"].includes(page)?"hidden":"auto" }}>
            {renderPage()}
          </div>
        </div>
      </div>

      {authModal && <AuthModal mode={authModal} onClose={()=>setAuthModal(null)}/>}
      <Toast/>
    </>
  );
}
