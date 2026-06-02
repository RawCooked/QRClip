# QRClip

**Envoyez instantanément un lien de votre PC vers votre téléphone via QR Code.**

Appuyez sur un raccourci clavier depuis n'importe quelle application — le QR Code apparaît immédiatement, prêt à être scanné.

---

## Comment ça marche

```
Ctrl+C  →  Ctrl+Shift+Espace  →  scanner  →  lien ouvert sur le téléphone
```

1. Copiez un lien avec `Ctrl+C`
2. Appuyez sur `Ctrl+Shift+Espace` (personnalisable)
3. Scannez le QR Code avec l'appareil photo de votre téléphone
4. Fermez avec `Échap` ou en cliquant ailleurs

L'application tourne silencieusement en arrière-plan dans le **system tray**.

---

## Prérequis

- Windows 10 ou 11 (64 bits)
- [.NET 8 Desktop Runtime x64](https://dotnet.microsoft.com/download/dotnet/8.0) — gratuit, ~55 MB

---

## Installation

### Option A — Exécutable (recommandé)

1. Téléchargez `QRClip.exe` depuis la page [Releases](../../releases)
2. Placez-le où vous voulez (ex. `C:\Program Files\QRClip\`)
3. Double-cliquez pour lancer — l'icône QR apparaît dans le system tray

> Pour le démarrage automatique avec Windows : clic droit sur l'icône → **Paramètres** → cocher *Lancer au démarrage*

### Option B — Compiler depuis les sources

**Prérequis :** [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
git clone https://github.com/votre-utilisateur/QRClip.git
cd QRClip
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Exécutable dans : `bin\Release\net8.0-windows\win-x64\publish\QRClip.exe`

---

## Utilisation

| Action | Comment |
|--------|---------|
| Afficher le QR Code | `Ctrl+Shift+Espace` (par défaut) |
| Fermer la popup | `Échap` ou clic hors fenêtre |
| Menu de l'application | Clic droit sur l'icône tray |
| Afficher le QR (sans clavier) | Double-clic sur l'icône tray |
| Ouvrir les paramètres | Clic droit → **Paramètres...** |

### Personnaliser le raccourci

Clic droit sur l'icône → **Paramètres...** → cliquer *Capturer* → appuyer sur la combinaison souhaitée.

La configuration est sauvegardée dans `%APPDATA%\QRClip\settings.json`.

---

## Désinstallation

1. Clic droit sur l'icône tray → **Quitter**
2. Supprimer `QRClip.exe`
3. *(Optionnel)* Supprimer `%APPDATA%\QRClip\`

---

## Technologies

- **C# / .NET 8 WPF** — natif Windows, léger, démarrage rapide
- **[QRCoder](https://github.com/codebude/QRCoder)** — génération QR en mémoire, sans fichier disque (MIT)
- **Win32 `RegisterHotKey`** — raccourci global intercepté avant toute application
- **Registre Windows** — démarrage automatique sans droits administrateur

---

## Roadmap

### v1.0 — Disponible
- [x] Raccourci clavier global configurable
- [x] QR Code généré depuis le presse-papiers
- [x] Popup overlay instantanée (Topmost, fermeture Échap)
- [x] System tray avec menu contextuel
- [x] Démarrage automatique avec Windows
- [x] Configuration persistante JSON

### v2.0 — Prévisions

#### Expérience utilisateur
- [ ] **Historique du presse-papiers** — 10 derniers QR générés, réaccessibles depuis le tray
- [ ] **Champ éditable** — modifier le texte avant génération (pour raccourcir une URL, ajouter un paramètre)
- [ ] **Bouton "Copier l'image QR"** — copier le QR Code lui-même dans le presse-papiers pour l'envoyer par email
- [ ] **Zoom ajustable** — glisser pour agrandir/réduire le QR dans la popup
- [ ] **Détection de type** — icône différente si URL, texte brut, email, Wi-Fi, numéro de téléphone

#### Apparence
- [ ] **Thème sombre automatique** — suivre le thème Windows (clair/sombre)
- [ ] **QR Code coloré** — couleur de premier plan personnalisable dans les paramètres
- [ ] **Animation d'apparition** — fondu discret à l'ouverture (< 80 ms)

#### Performance & stabilité
- [ ] **Instance unique** — protection Mutex pour éviter plusieurs copies en arrière-plan
- [ ] **Single-file publish** — `.exe` autonome sans dépendance, déployable par simple copie
- [ ] **Réduction mémoire** — libérer les ressources QR après fermeture de la popup

#### Intégration Windows
- [ ] **Notifications toast Windows 11** — via WinRT, plus moderne que les ballons tray
- [ ] **Menu Jumplist** — "Afficher QR" directement depuis la barre des tâches (clic droit sur l'icône)
- [ ] **Protocole URI** `qrclip://` — ouvrir depuis un script ou une autre application
- [ ] **Installateur MSIX** — installation propre via le Microsoft Store ou en entreprise

#### Fonctionnalités avancées
- [ ] **Raccourcis multiples** — un raccourci pour le presse-papiers, un autre pour l'historique
- [ ] **Mode partage Wi-Fi** — générer un QR de connexion Wi-Fi depuis un SSID détecté
- [ ] **Mise à jour automatique** — vérification de nouvelles versions sur GitHub Releases

---

## Licence

MIT
