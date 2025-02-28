using Loquit.Data.Entities;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    public class PostRepository : CrudRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsByAlgorithmAsync(bool allowNsfw, double[] categoryPreferences, double[] evaluationPreferences, int[] recenlyOpedenPostsIds)
        {
            var postsCount = _context.Posts.Count();

            List<Post> posts;

            if (!allowNsfw)
            {
                if (postsCount >= 50)
                {
                    posts = await _context.Posts
                        .Where(item => item.IsNsfw == false)
                        .Where(item => !recenlyOpedenPostsIds.Contains(item.Id))
                        .ToListAsync();
                }
                else
                {
                    posts = await _context.Posts
                        .Where(item => item.IsNsfw == false)
                        .ToListAsync();
                }

                posts = posts
                    .OrderBy(item => categoryPreferences[TransformToNumber(item.Category)] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                    .Take(50)
                    .ToList();

                return posts;
            }
            else
            {
                if (postsCount >= 50)
                {
                    posts = await _context.Posts
                        .Where(item => !recenlyOpedenPostsIds.Contains(item.Id))
                        .ToListAsync();
                }
                else
                {
                    posts = await _context.Posts
                        .ToListAsync();
                }

                posts = posts
                    .OrderBy(item => categoryPreferences[TransformToNumber(item.Category)] + item.Evaluations[0] * evaluationPreferences[0] + item.Evaluations[1] * evaluationPreferences[1] + item.Evaluations[2] * evaluationPreferences[2] + item.Evaluations[3] * evaluationPreferences[3] + item.Evaluations[4] * evaluationPreferences[4]/* + Math.Tanh((item.Likes - item.Dislikes) / 10000)*/)
                    .Take(50)
                    .ToList();

                return posts;
            }
        }

        public double[] Evaluate(Post post)
        {
            string title = post.Title.ToLower();
            string bodyText = post.BodyText.ToLower();
            string[] eval1 = { "government", "election", "policy", "democracy", "republic", "senate", "congress", "campaign", "ideology", "legislation", "diplomacy", "parliament", "constitution", "authority", "lobbying", "bureaucracy", "reform", "conservative", "liberal", "socialism", "capitalism", "nationalism", "populism", "autocracy", "dissent", "propaganda", "vote", "referendum", "partisanship", "executive", "judiciary", "judgment", "censorship", "leadership", "protest", "movement", "coalition", "manifesto", "sovereignty", "federalism", "impeachment", "taxation", "filibuster", "monarchy", "scandal", "diplomatic", "campaign finance", "military-industrial complex", "activism", "war", "treaty", "legislature", "gerrymandering", "lobbyist", "amendment", "left-wing", "right-wing", "midterms", "judicial review", "checks and balances", "red tape", "political correctness", "free speech", "libertarian", "neoliberalism", "anarchism", "authoritarianism", "human rights", "civil liberties", "statecraft", "policy-making", "diplomatic immunity", "constitutional crisis", "polarization", "misinformation", "conspiracy", "boycott", "free speech", "whistleblower", "gun control", "abortion", "vaccines", "climate change", "AI ethics", "data privacy", "genetic modification", "hate speech", "social justice", "BLM", "MeToo", "deepfakes", "fake news", "intellectual property", "monopoly", "big tech", "euthanasia", "immigration", "capital punishment", "gentrification", "war crimes", "child labor", "prison reform", "economic inequality", "hate crime", "sex work", "religious freedom", "media bias", "whistleblowing", "espionage", "martial law", "surveillance state", "state propaganda", "black ops", "trade war", "soft power", "hard power", "political asylum", "ethnic cleansing", "diplomatic boycott", "political dynasty", "sovereign debt", "federal reserve", "protectionism", "foreign interference", "humanitarian crisis", "civil disobedience", "peacekeeping", "lame duck", "grassroots", "silent majority", "majoritarianism", "diplomatic recognition", "militarization", "press freedom", "shadow government", "executive order", "provisional government", "regime change", "patriotism", "national anthem", "populist uprising", "congressional hearing", "classified documents", "public sector", "corporate lobbying", "dark money", "voter suppression", "judicial activism", "protest movement", "martyrdom", "foreign policy", "national security", "diplomatic relations", "policy debate", "international sanctions", "regulatory oversight", "secession", "succession crisis", "oligarchy", "kleptocracy", "political satire", "civil resistance", "deep state", "grassroots activism", "political polarization", "social contract", "political spectrum", "legal precedent", "executive privilege", "policy reform", "power dynamics", "checks and balances", "watchdog journalism", "constitutional amendment", "fiscal policy", "judicial independence", "press censorship", "sanctions", "debt ceiling", "foreign aid", "parliamentary immunity", "constitutional monarchy", "international law", "diplomatic negotiations", "foreign espionage", "covert operations", "diplomatic tension", "ambassador", "national security advisor", "political scandal", "revolution", "lobbying group", "opposition party", "economic sanctions", "arms control", "nation-state", "public opinion", "voter turnout", "political corruption", "electoral fraud", "geopolitical rivalry", "political imprisonment", "diplomatic pressure", "military occupation", "global governance", "peace talks", "separatism", "political polarization", "treaty violation", "geostrategy", "imperialism", "humanitarian aid", "legislative impasse", "diplomatic summit", "political maneuvering", "social reform", "civil service", "coup attempt", "state sovereignty", "redistricting", "political identity", "foreign intervention", "diplomatic envoy", "media manipulation", "state-sponsored hacking", "propaganda warfare", "international tribunal", "diplomatic breakthrough", "political rhetoric", "soft coup", "hard coup", "diplomatic scandal", "constitutional law", "political negotiation", "opposition leader", "economic nationalism", "political realignment", "election interference", "bipartisanship", "military strategy", "political asylum seeker", "embargo", "nation-building", "voting rights", "grassroots mobilization", "welfare state", "political factions", "geopolitical landscape", "national referendum", "political coalition", "public policy", "international diplomacy", "domestic policy", "state intervention", "electoral college", "political philosophy", "rhetorical strategy", "media framing", "nuclear deterrence", "executive branch", "government shutdown", "federal oversight", "political ideologies", "think tank", "power struggle", "diplomatic leverage", "civil unrest", "global conflict", "political patronage", "strategic alliance", "war on terror", "nationalism vs. globalism" };
            string[] eval2 = { "happy", "sad", "anxious", "excited", "melancholy", "angry", "content", "fearful", "hopeful", "irritated", "joyful", "depressed", "serene", "frustrated", "nervous", "tense", "elated", "calm", "restless", "miserable", "grateful", "confident", "lonely", "cheerful", "apathetic", "pensive", "energetic", "overwhelmed", "relaxed", "enthusiastic", "guilty", "nostalgic", "moody", "worried", "bored", "euphoric", "resentful", "sentimental", "optimistic", "gloomy", "whimsical", "heartwarming", "bittersweet", "reminiscent", "wistful", "soulful", "reflective", "yearning", "homesick", "daydreaming", "longing", "cherished", "beloved", "romanticized", "wistful", "sweet", "innocent", "idealized", "vintage", "old-school", "retro", "classic", "throwback", "timeless", "familiar", "golden", "old-fashioned", "vibrant", "rustic", "sentiment", "memory", "flashback", "recollection", "echo", "past", "childhood", "simpler times", "storybook", "rose-tinted", "warmth", "comforting", "cozy", "faded", "soft", "gentle", "tender", "silhouette", "hazy", "flickering", "grandmother’s house", "sepia-toned", "vhs tapes", "cassette", "mixtape", "jukebox", "roller skates", "old photographs", "record player", "handwritten letters", "typewriter", "vinyl", "Super 8", "arcade", "Polaroid", "scrapbook", "diary", "fairy lights", "love letters", "summer camp", "sleepovers", "scented markers", "80s", "90s", "Y2K", "classic rock", "folk music", "radio static", "walkman", "boom box", "faded jeans", "mom’s perfume", "dad’s old car", "first bike", "drive-in movie", "hand-me-downs", "photo album", "Christmas morning", "Sunday mornings", "cartoon reruns", "old sitcoms", "black-and-white films", "silent movies", "fairy tales", "lullabies", "storybook endings", "childhood dreams", "snow days", "pillow forts", "board games", "family dinners", "chocolate chip cookies", "homemade", "quilts", "grandfather clock", "porch swings", "sunsets", "fireflies", "barefoot in the grass", "warm hugs", "favorite sweater", "wool socks", "comfort food", "handwritten notes", "pressed flowers", "first love", "first kiss", "first heartbreak", "diary entries", "passing notes in class", "high school crush", "locker decorations", "yearbooks", "prom night", "wishing on stars", "birthday candles", "friendship bracelets", "sleepover secrets", "the smell of old books", "library visits", "storybook worlds", "train rides", "summer vacations", "penny candy", "playgrounds", "rope swings", "treehouses", "skipping rocks", "lake days", "beach bonfires", "picnics", "road trips", "small towns", "corner stores", "ice cream trucks", "neon signs", "drive-thru diners", "jukebox music", "poodle skirts", "sock hops", "50s diner", "carnival rides", "ferris wheel", "cotton candy", "parades", "classic cars", "letterman jackets", "football games", "paper routes", "Sunday comics", "cartoon cereal boxes", "morning cartoons", "bubblegum pop", "lava lamps", "beanie babies", "furbies", "Lisa Frank", "Tamagotchis", "game boy", "early internet", "dial-up", "instant messaging", "aol", "away messages", "emo phase", "skater culture", "mall culture", "MySpace", "friendship pacts", "secret handshakes", "youthful innocence", "summer flings", "junior high drama", "simpler joys", "timeless magic" };
            string[] eval3 = { "technology", "disruption", "breakthrough", "progress", "AI", "automation", "startups", "blockchain", "biotech", "nanotechnology", "quantum computing", "cybernetics", "VR", "AR", "robotics", "design", "invention", "sustainability", "clean energy", "future", "R&D", "engineering", "space exploration", "biometrics", "genetics", "cryptocurrency", "networking", "machine learning", "5G", "smart cities", "electric vehicles", "fintech", "IoT", "cloud computing", "green tech", "wearables", "personalization", "deep learning", "hyperloop", "metaverse", "big data", "neural networks", "bioengineering", "self-driving cars", "fusion energy", "quantum internet", "vertical farming", "carbon capture", "precision medicine", "brain-computer interface", "gene editing", "CRISPR", "robotic surgery", "regenerative medicine", "haptic technology", "smart materials", "exoskeletons", "drones", "swarm intelligence", "AI ethics", "automated factories", "digital twins", "3D printing", "space tourism", "Mars colonization", "flying cars", "renewable energy", "biodegradable plastics", "wireless energy transfer", "smart grids", "augmented intelligence", "holography", "nanorobots", "cybersecurity advancements", "next-gen batteries", "bioluminescent lighting", "quantum encryption", "AI-generated content", "wearable health monitors", "bionics", "lab-grown meat", "zero-gravity manufacturing", "neuroscience innovations", "internet of behaviors", "human augmentation", "AI co-creativity", "digital ecosystems", "brain mapping", "AI pharmacology", "quantum-resistant cryptography", "ethical hacking", "bioprinting", "holographic displays", "space elevators", "bioelectronics", "cognitive computing", "sentient AI", "ambient intelligence", "futuristic transportation", "nano-medicine", "fusion reactors", "sustainable architecture", "AI-driven governance", "lifespan extension", "personalized AI assistants", "alternative protein sources", "self-repairing materials", "solar-powered desalination", "distributed ledgers", "decentralized internet", "hyper-realistic AI avatars", "smart prosthetics", "digital sovereignty", "quantum supremacy", "automated legal systems", "DNA computing", "AI-driven creativity", "next-gen cybersecurity", "AI-powered agriculture", "suborbital flights", "mind-controlled technology", "AI-mediated diplomacy", "real-time language translation", "anti-aging breakthroughs", "interplanetary mining", "self-healing concrete", "ocean energy", "robotic swarm technology", "AI-assisted music composition", "modular housing", "brainwave-controlled devices", "deep space exploration", "post-quantum encryption", "fusion-powered spacecraft", "next-gen biofuels", "VR therapy", "automated journalism", "circular economy", "biodigital convergence", "autonomous public transport", "wireless brain implants", "molecular manufacturing", "lunar bases", "digital immortality", "self-assembling nanostructures", "holographic AI assistants" };
            string[] eval4 = { "fashion", "viral", "aesthetic", "lifestyle", "minimalism", "influencer", "TikTok", "social media", "wellness", "sustainability", "vegan", "streetwear", "luxury", "fast fashion", "DIY", "body positivity", "self-care", "digital detox", "AI art", "mindfulness", "fitness", "Y2K", "vintage", "genderless fashion", "athleisure", "eco-friendly", "NFTs", "microtrends", "subscription economy", "plant-based", "alternative", "thrifting", "indie style", "personal branding", "astrology", "cozy living", "cottagecore", "dopamine dressing", "hybrid work", "slow living", "quiet luxury", "deinfluencing", "clean girl aesthetic", "dark academia", "light academia", "mob wife aesthetic", "balletcore", "mermaidcore", "gorpcore", "barbiecore", "ugly chic", "normcore", "old money aesthetic", "techwear", "AI-generated fashion", "3D-printed clothing", "pre-loved fashion", "capsule wardrobe", "work-from-anywhere", "coffee culture", "matcha craze", "digital nomad", "crypto culture", "gaming culture", "esports", "k-pop influence", "anime aesthetics", "hyperpop", "booktok", "film photography revival", "claw clips", "scrunchies", "nostalgia marketing", "2000s comeback", "90s revival", "sneaker culture", "hiking boom", "outdoor living", "plant parenthood", "air fryer recipes", "fermented foods", "gut health", "skincare boom", "glass skin", "glazed donut nails", "bold brows", "red nail theory", "AI influencers", "personalized skincare", "robot baristas", "mental health awareness", "burnout recovery", "dopamine dressing", "bold patterns", "crochet fashion", "handmade jewelry", "statement earrings", "pearlcore", "silver jewelry comeback", "minimalist tattoos", "stick-and-poke tattoos", "scalp care", "slugging", "hair cycling", "DIY hair dye", "microblading", "bleached brows", "chrome nails", "bubble nails", "clean beauty", "sustainable packaging", "refillable beauty", "scandi aesthetic", "French girl style", "old Hollywood revival", "red lipstick resurgence", "Goth glam", "spiky hair", "mushroom leather", "lab-grown diamonds", "luxury reselling", "influencer brands", "collagen craze", "biohacking", "no-alcohol movement", "zero-proof drinks", "matcha lattes", "gut-friendly drinks", "electrolyte drinks", "brain food", "adaptogens", "micro-dosing", "psychedelic therapy", "VR concerts", "hologram concerts", "AI-generated music", "fan-made AI covers", "Spotify wrapped obsession", "concept albums", "vinyl revival", "cassettes comeback", "AI DJs", "digital fashion", "wearable NFTs", "metaverse fashion", "AI-generated avatars", "personal AI assistants", "AI dating apps", "virtual influencers", "subscription-only social media", "paywalled content", "exclusive communities", "Twitter alternatives", "text-based social media", "decentralized apps", "VR workspaces", "AI tutors", "voice-first apps", "AI-generated essays", "productivity hacks", "pomodoro technique", "habit stacking", "finfluencers", "financial literacy content", "side hustle culture", "remote work evolution", "work-life balance", "ghost jobs", "quiet quitting", "loud quitting", "rage applying", "boomerang employees", "digital gardens", "personal wikis", "second brains", "notion templates", "aesthetic note-taking", "e-ink tablets", "paperless journaling", "self-improvement culture", "mental health tracking", "journaling revival", "sound healing", "breathwork", "hot girl walks", "manifestation", "vision boards", "gratitude practices", "moon rituals", "spiritual awakening", "numerology", "angel numbers", "law of assumption", "intuitive eating", "gut-brain connection", "no-makeup makeup", "soft girl aesthetic", "boyfriend core", "grunge revival", "alt-girl aesthetic", "cybercore", "techno fashion", "futuristic looks", "body chains", "DIY thrift flips", "fast fashion backlash", "celebrity skincare brands", "fragrance layering", "hair oiling", "heatless curls", "rice water hair trend", "high-tech fabrics", "LED face masks", "infrared saunas", "wellness tech", "biohacking wearables" };
            string[] eval5 = { "fun fact", "random fact", "did you know", "history", "science", "geography", "space", "biology", "physics", "chemistry", "animals", "ocean", "deep sea", "famous inventors", "scientific discoveries", "unknown facts", "mythology", "ancient civilizations", "Egyptians", "Romans", "Vikings", "astronomy", "black holes", "neutron stars", "time travel theories", "parallel universes", "quantum physics", "Schrödinger’s cat", "famous last words", "weird laws", "strange customs", "world records", "fastest animals", "tallest mountains", "deepest oceans", "deadliest creatures", "famous hoaxes", "optical illusions", "brain teasers", "riddles", "mysteries", "conspiracy theories", "unsolved crimes", "Bermuda Triangle", "Atlantis", "Loch Ness Monster", "Bigfoot", "Mandela effect", "Déjà vu", "strange coincidences", "UFOs", "alien theories", "extraterrestrial life", "Mars exploration", "Moon landing", "fossils", "dinosaurs", "Jurassic period", "evolution", "genetic mutations", "oldest trees", "longest-living animals", "weirdest foods", "rare diseases", "incredible human feats", "ancient languages", "lost cities", "hidden treasures", "mysterious deaths", "phobias", "placebo effect", "strange animal behaviors", "camouflage", "bioluminescence", "self-cloning animals", "deep-sea creatures", "ocean trenches", "space travel", "strangest planets", "Mars’ weather", "volcanoes on Venus", "dark matter", "multiverse theories", "infinity paradox", "sound in space", "gravity anomalies", "oldest fossils", "extinct animals", "de-extinction science", "genetic engineering", "cloning", "weirdest sports", "strange world records", "human body facts", "tallest people", "shortest people", "record-breaking weather", "windiest place", "coldest place", "hottest place", "fastest cars", "first inventions", "origin of words", "language facts", "funny mistranslations", "weirdest traditions", "odd holidays", "quirky historical events", "strange royal rules", "historical coincidences", "little-known wars", "famous spies", "unsolved mysteries", "haunted places", "urban legends", "cursed objects", "weirdest phobias", "strangest psychological experiments", "hidden rooms", "codes and ciphers", "secret societies", "mysterious books", "Voynich Manuscript", "ancient technology", "forgotten inventions", "failed predictions", "future tech", "unexpected scientific discoveries", "accidental inventions", "bizarre animals", "weird natural phenomena", "glowing beaches", "rainbow mountains", "singing sands", "blood falls", "animal superpowers", "plants that move", "plants that kill", "oldest books", "first languages", "underground cities", "hidden tunnels", "secret passageways" };
            int[] evals = { 1, 1, 1, 1, 1 };
            double total = 0;
            foreach (string phrase in eval1)
            {
                if (title.Contains(phrase)) { evals[0] += 3; }
                if (bodyText.Contains(phrase)) { evals[0] += 1; }
            }
            foreach (string phrase in eval2)
            {
                if (title.Contains(phrase)) { evals[1] += 3; }
                if (bodyText.Contains(phrase)) { evals[1] += 1; }
            }
            foreach (string phrase in eval3)
            {
                if (title.Contains(phrase)) { evals[2] += 3; }
                if (bodyText.Contains(phrase)) { evals[2] += 1; }
            }
            foreach (string phrase in eval4)
            {
                if (title.Contains(phrase)) { evals[3] += 3; }
                if (bodyText.Contains(phrase)) { evals[3] += 1; }
            }
            foreach (string phrase in eval5)
            {
                if (title.Contains(phrase)) { evals[4] += 3; }
                if (bodyText.Contains(phrase)) { evals[4] += 1; }
            }
            total = evals.Sum();
            return [Math.Round(double.Parse($"{evals[0] / total}"), 2), Math.Round(double.Parse($"{evals[1] / total}"), 2), Math.Round(double.Parse($"{evals[2] / total}"), 2), Math.Round(double.Parse($"{evals[3] / total}"), 2), Math.Round(double.Parse($"{evals[4] / total}"), 2)];
        }

        private int TransformToNumber(string input)
        {
            switch (input)
            {
                case "Food":
                    return 1;
                case "Music/Art":
                    return 2;
                case "Nature":
                    return 3;
                case "Pets":
                    return 4;
                case "Videogames":
                    return 5;
                case "Culture":
                    return 6;
                case "Funny":
                    return 7;
                case "Factology":
                    return 8;
                default:
                    return 0;
            }
        }
    }
}
