# 🧩 **__RSUnityFramework__**
> _Reusable modular foundation for Unity projects — by RS Studio_  
> _Last updated: October 22, 2025_

---

## 📚 Table of Contents
1. [Overview](#-overview)
2. [Folder Structure](#-folder-structure)
3. [Folder Overview](#-folder-overview)
4. [Module Details](#-module-details)
   - [Common](#-common)
   - [Core](#-core)
   - [Editor](#-editor)
   - [Managers](#-managers)
   - [Services](#-services)
   - [UI](#-ui)
   - [Utilities](#-utilities)
   - [Plugins](#-plugins)
5. [Integration Guidelines](#-integration-guidelines)
6. [Coding Standards](#-coding-standards)
7. [Design Philosophy](#-design-philosophy)
8. [Notes](#-notes)

---

## 🌐 Overview
`__RSUnityFramework__` is the **core shared framework** used across all Unity projects at RS Studio.  
It provides a **modular architecture**, standardized **service layer**, and **utility toolkit** to accelerate development and ensure consistency between projects.

---

## 📁 Folder Structure

### **RSUnityFramework/**
- **📂 Common/**  
  Base types, extensions, and helper utilities.
- **📂 Core/**  
  Service Locator, BaseManager, and optional Dependency Injection system.
- **📂 Editor/**  
  Custom editor tools, inspectors, and menu utilities.
- **📂 Managers/**  
  Central managers (GameManager, UIManager, SceneManager, etc.).
- **📂 Services/**  
  Global systems such as Audio, Save, Analytics, RemoteConfig, and Localization.
- **📂 UI/**  
  Base UI logic and reusable components (screens, popups, etc.).
- **📂 Utilities/**  
  Generic tools for Math, Event, Coroutine, Tween, and FileIO.
- **📂 Plugins/**  
  Third-party dependencies (DOTween, Odin, Firebase, etc.).

---

## 🧭 Folder Overview

| Folder | Description |
|--------|--------------|
| **Common** | Shared base types, helpers, and extension methods used across the framework. |
| **Core** | Foundation layer containing Service Locator, BaseManager, and optional DI system. |
| **Editor** | Custom Unity Editor tools, inspectors, and utilities for internal workflows. |
| **Managers** | Core managers that handle game state, UI flow, and scene transitions. |
| **Services** | Independent service modules for audio, saving, analytics, remote configs, and localization. |
| **UI** | Base UI architecture and shared components for screens and popups. |
| **Utilities** | Lightweight reusable tools for math, file IO, events, and coroutines. |
| **Plugins** | External libraries integrated into the framework (DOTween, Odin, Firebase, etc.). |

---

## ⚙️ Module Details

### 🧱 **Common/**
Contains shared definitions and basic helper classes.
- `Extensions/` – Common C# and Unity extension methods.  
- `Helpers/` – Generic helper functions (math, IO, string, etc.).  
- `Constants/` – Shared constants and enums.

---

### 🧠 **Core/**
Foundation systems for dependency management and initialization.
- `ServiceLocator` – Centralized access to global services.  
- `BaseManager` – Common base for managers.  
- `DependencyInjection` – Optional DI or lifetime management system.

> 🧩 This layer connects services, managers, and gameplay systems.

---

### 🧰 **Editor/**
Editor-time utilities and tools.  
- Custom inspectors and property drawers.  
- Odin integration scripts.  
- Menu and asset creation tools.  

> ⚠️ Automatically excluded from runtime builds.

---

### 🎮 **Managers/**
Core managers that control runtime systems.  
- `GameManager` – Entry point for the main game loop.  
- `UIManager` – Handles screen transitions and popup flow.  
- `SceneManager` – Manages scene transitions and loading.

> Each manager should remain **game-agnostic** and **reusable**.

---

### 🌍 **Services/**
Independent systems that manage cross-game data or logic:  
- `AudioService` – Music and SFX management.  
- `SaveService` – Player save & load logic.  
- `AnalyticsService` – Tracking and analytics wrappers.  
- `RemoteConfigService` – Runtime configuration management.  
- `LocalizationService` – Multi-language text and asset support.

> All services implement a common interface (e.g. `IService`)  
> and register themselves via the `ServiceLocator` or `ServiceManager`.

---

### 🖼️ **UI/**
Base UI components and view logic.  
- `UIBase` – Base class for all UI screens.  
- `UIComponent` – Reusable UI elements (buttons, progress bars, etc.).  
- `PopupBase` – Modal or popup window base class.

> Recommended architecture: **MVVM** or **MVC** pattern.

---

### 🔧 **Utilities/**
General-purpose reusable utilities.  
- `Math/` – Math helpers, interpolation, random utils.  
- `Time/` – Timer, cooldown helpers.  
- `Event/` – Event bus, observer system.  
- `Tween/` – DOTween helpers or custom tweens.  
- `Coroutine/` – Static coroutine runner.  
- `FileIO/` – File read/write, JSON, and serialization tools.

> Should be lightweight and independent of game logic.

---

### 📦 **Plugins/**
Third-party libraries and SDKs integrated into the project.  
- `DOTween/` – Tweening engine.  
- `OdinInspector/` – Editor enhancement tools.  
- `Firebase/`, `GameAnalytics/`, etc.

> 🧭 Keep all plugin code isolated.  
> Use wrapper classes in **Services/** or **Utilities/** for maintainability.

---

## 🧩 Integration Guidelines

1. **Do not modify** plugin or third-party source code.  
   → Use wrapper classes in `Services/` or `Utilities/` instead.

2. **All services** must implement a shared interface (`IService`)  
   and register themselves via `ServiceLocator`.

3. **Framework updates** must remain **game-agnostic** and **backward compatible**.

4. Keep each module **self-contained** for easier maintenance and versioning.

---

## 🧭 Coding Standards

| Convention | Example |
|-------------|----------|
| **Namespace format** | `RSUnity.Services.Audio`, `RSUnity.UI.Components` |
| **Script naming** | Match class name exactly (PascalCase) |
| **File organization** | One class per file unless strongly related |
| **Data storage** | Prefer `ScriptableObjects` for configuration |
| **Initialization** | Use `[RuntimeInitializeOnLoadMethod]` for auto-registration |

---

## 🧱 Design Philosophy

> _“Write once, reuse everywhere.”_  
> `__RSUnityFramework__` is built for **scalability**, **modularity**, and **reusability**.  
> Any feature that could be reused in another project should be developed here.

---

## 🏷️ Notes
- `__RSUnityFramework__` must remain **independent of game-specific content**.  
- Designed to be imported or version-controlled as a shared framework module.  
- Ensure compatibility with Unity **6000.0.60f1 (LTS)** or higher.

---

### © RS Studio – Internal Framework Documentation
Maintained by the **RS Core Engineering Team**  
_This document is intended for internal use only._
